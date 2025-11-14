using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace CodingAgent.Tests;

public class AgentInstructionsTests
{
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
}