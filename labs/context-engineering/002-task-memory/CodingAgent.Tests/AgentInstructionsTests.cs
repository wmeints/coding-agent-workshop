using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace CodingAgent.Tests;

public class AgentInstructionsTests : IDisposable
{
    private readonly string _tempDirectory;

    public AgentInstructionsTests()
    {
        _tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_tempDirectory);
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempDirectory))
        {
            Directory.Delete(_tempDirectory, recursive: true);
        }
    }

    [Fact]
    public async Task InjectAsync_ShouldIncludeOperatingSystemAndWorkingDirectory()
    {
        // Arrange
        var agentInstructions = new AgentInstructions();
        var context = new AgentContext
        {
            WorkingDirectory = "/home/user/projects",
            OperatingSystem = "Linux"
        };

        var kernel = Kernel.CreateBuilder().Build();
        var chatHistory = new ChatHistory();

        // Act
        await agentInstructions.InjectAsync(kernel, context, chatHistory);

        // Assert
        Assert.Single(chatHistory);
        Assert.Equal(AuthorRole.System, chatHistory[0].Role);
        Assert.Contains("/home/user/projects", chatHistory[0].Content);
        Assert.Contains("Linux", chatHistory[0].Content);
    }

    [Fact]
    public async Task InjectAsync_WithCustomInstructions_ShouldIncludeDeveloperMessage()
    {
        // Arrange
        var customInstructions = "# Custom Instructions\nAlways use async/await patterns.";
        var agentsFilePath = Path.Combine(_tempDirectory, "AGENTS.md");
        await File.WriteAllTextAsync(agentsFilePath, customInstructions);

        var agentInstructions = new AgentInstructions();
        var context = new AgentContext
        {
            WorkingDirectory = _tempDirectory,
            OperatingSystem = "Linux"
        };

        var kernel = Kernel.CreateBuilder().Build();
        var chatHistory = new ChatHistory();

        // Act
        await agentInstructions.InjectAsync(kernel, context, chatHistory);

        // Assert
        Assert.Equal(2, chatHistory.Count);
        Assert.Equal(AuthorRole.System, chatHistory[0].Role);
        Assert.Equal(AuthorRole.Developer, chatHistory[1].Role);
        Assert.Equal(customInstructions, chatHistory[1].Content);
    }

    [Fact]
    public void Remove_WithCustomInstructions_ShouldRemoveSystemAndDeveloperMessages()
    {
        // Arrange
        var agentInstructions = new AgentInstructions();
        var chatHistory = new ChatHistory();
        chatHistory.Add(new ChatMessageContent(AuthorRole.System, "System prompt"));
        chatHistory.Add(new ChatMessageContent(AuthorRole.Developer, "Custom instructions"));
        chatHistory.Add(new ChatMessageContent(AuthorRole.User, "User message"));

        // Act
        agentInstructions.Remove(chatHistory);

        // Assert
        Assert.Single(chatHistory);
        Assert.Equal(AuthorRole.User, chatHistory[0].Role);
        Assert.Equal("User message", chatHistory[0].Content);
    }

    [Fact]
    public void Remove_WithoutCustomInstructions_ShouldRemoveOnlySystemMessage()
    {
        // Arrange
        var agentInstructions = new AgentInstructions();
        var chatHistory = new ChatHistory();
        chatHistory.Add(new ChatMessageContent(AuthorRole.System, "System prompt"));
        chatHistory.Add(new ChatMessageContent(AuthorRole.User, "User message"));

        // Act
        agentInstructions.Remove(chatHistory);

        // Assert
        Assert.Single(chatHistory);
        Assert.Equal(AuthorRole.User, chatHistory[0].Role);
        Assert.Equal("User message", chatHistory[0].Content);
    }
}