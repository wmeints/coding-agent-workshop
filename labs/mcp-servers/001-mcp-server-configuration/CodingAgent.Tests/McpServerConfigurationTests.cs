using CodingAgent.Configuration;

namespace CodingAgent.Tests;

public class McpServerConfigurationTests : IDisposable
{
    private readonly List<string> _tempDirectories = [];
    private readonly List<string> _environmentVariablesToCleanup = [];

    public void Dispose()
    {
        foreach (var envVar in _environmentVariablesToCleanup)
        {
            Environment.SetEnvironmentVariable(envVar, null);
        }

        foreach (var tempDir in _tempDirectories)
        {
            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, recursive: true);
            }
        }
    }

    private string CreateTempDirectory()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDir);
        _tempDirectories.Add(tempDir);
        return tempDir;
    }

    private void SetEnvironmentVariable(string name, string value)
    {
        Environment.SetEnvironmentVariable(name, value);
        _environmentVariablesToCleanup.Add(name);
    }

    [Fact]
    public async Task Load_WithValidMcpJsonFile_ReturnsConfiguration()
    {
        // Arrange
        var tempDir = CreateTempDirectory();
        var agentDir = Path.Combine(tempDir, ".agent");
        Directory.CreateDirectory(agentDir);

        var mcpJson = """
            {
                "test-server": {
                    "command": "npx",
                    "args": ["-y", "@test/mcp-server"]
                }
            }
            """;

        await File.WriteAllTextAsync(Path.Combine(agentDir, "mcp.json"), mcpJson);

        // Act
        var configuration = await McpServerConfiguration.Load(tempDir);

        // Assert
        Assert.NotNull(configuration);
        Assert.Single(configuration.Servers);
        Assert.True(configuration.Servers.ContainsKey("test-server"));

        var server = configuration.Servers["test-server"];
        
        Assert.Equal("npx", server.Command);
        Assert.NotNull(server.Arguments);
        Assert.Equal(2, server.Arguments.Count);
        Assert.Equal("-y", server.Arguments[0]);
        Assert.Equal("@test/mcp-server", server.Arguments[1]);
    }

    [Fact]
    public async Task Load_WithEnvironmentVariablePlaceholder_ResolvesPlaceholder()
    {
        // Arrange
        var tempDir = CreateTempDirectory();
        var agentDir = Path.Combine(tempDir, ".agent");
        Directory.CreateDirectory(agentDir);

        const string testEnvVarName = "MCP_TEST_API_KEY";
        const string testEnvVarValue = "secret-api-key-12345";
        SetEnvironmentVariable(testEnvVarName, testEnvVarValue);

        var mcpJson = """
            {
                "test-server": {
                    "command": "node",
                    "args": ["server.js"],
                    "env": {
                        "API_KEY": "${MCP_TEST_API_KEY}"
                    }
                }
            }
            """;

        await File.WriteAllTextAsync(Path.Combine(agentDir, "mcp.json"), mcpJson);

        // Act
        var configuration = await McpServerConfiguration.Load(tempDir);

        // Assert
        Assert.NotNull(configuration);
        var server = configuration.Servers["test-server"];
        Assert.NotNull(server.EnvironmentVariables);
        Assert.Equal(testEnvVarValue, server.EnvironmentVariables["API_KEY"]);
    }

    [Fact]
    public async Task Load_WithHttpServerConfiguration_ReturnsConfiguration()
    {
        // Arrange
        var tempDir = CreateTempDirectory();
        var agentDir = Path.Combine(tempDir, ".agent");
        Directory.CreateDirectory(agentDir);

        var mcpJson = """
            {
                "http-server": {
                    "url": "http://localhost:8080/mcp"
                }
            }
            """;

        await File.WriteAllTextAsync(Path.Combine(agentDir, "mcp.json"), mcpJson);

        // Act
        var configuration = await McpServerConfiguration.Load(tempDir);

        // Assert
        Assert.NotNull(configuration);
        Assert.Single(configuration.Servers);
        Assert.True(configuration.Servers.ContainsKey("http-server"));

        var server = configuration.Servers["http-server"];
        Assert.Equal("http://localhost:8080/mcp", server.Url);
    }

    [Fact]
    public async Task Load_WithMissingFile_ThrowsException()
    {
        // Arrange
        var tempDir = CreateTempDirectory();

        // Act & Assert
        // DirectoryNotFoundException is thrown when .agent directory doesn't exist
        await Assert.ThrowsAsync<DirectoryNotFoundException>(
            () => McpServerConfiguration.Load(tempDir));
    }
}
