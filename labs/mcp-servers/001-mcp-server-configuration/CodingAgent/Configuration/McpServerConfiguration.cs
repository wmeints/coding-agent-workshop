using System.Text.Json;
using System.Text.RegularExpressions;
using ModelContextProtocol.Client;

namespace CodingAgent.Configuration;

public class McpServerConfiguration
{
    private McpServerConfiguration(Dictionary<string, McpServerConfigurationItem> servers)
    {
        Servers = servers;
    }

    public Dictionary<string, McpServerConfigurationItem> Servers { get; }

    public static async Task<McpServerConfiguration> Load(string baseDirectory)
    {
        var configurationFilePath = Path.Join(baseDirectory, ".agent", "mcp.json");
        await using var fileStream = File.OpenRead(configurationFilePath);

        var configuration =
            await JsonSerializer.DeserializeAsync<Dictionary<string, McpServerConfigurationItem>>(fileStream);

        if (configuration is null)
        {
            throw new InvalidOperationException("Failed to load MCP server configuration.");
        }

        foreach (var (key, config) in configuration)
        {
            // Resolve any placeholders ${ENV_VAR} in environment variables.
            // This allows sensitive information to be stored outside the configuration file.
            if (config.EnvironmentVariables is { } environmentVariables)
            {
                config.EnvironmentVariables = ResolveEnvironmentVariables(environmentVariables);
            }
        }

        return new McpServerConfiguration(configuration!);
    }

    public async Task<List<McpClient>> CreateAllClientsAsync()
    {
        var clients = new List<McpClient>();

        foreach (var serverConfiguration in Servers)
        {
            clients.Add(await serverConfiguration.Value.CreateClientAsync());
        }

        return clients;
    }

    private static Dictionary<string, string?> ResolveEnvironmentVariables(
        Dictionary<string, string?> environmentVariables)
    {
        var results = new Dictionary<string, string?>();
        var pattern = new Regex("^\\${(.+?)}$");

        foreach (var keyValuePair in environmentVariables)
        {
            if (!string.IsNullOrEmpty(keyValuePair.Value) &&
                pattern.Match(keyValuePair.Value) is { Success: true } match)
            {
                var environmentVariableValue = Environment.GetEnvironmentVariable(match.Groups[1].Value);
                results.Add(keyValuePair.Key, environmentVariableValue);
                continue;
            }

            results.Add(keyValuePair.Key, keyValuePair.Value);
        }

        return results;
    }
}