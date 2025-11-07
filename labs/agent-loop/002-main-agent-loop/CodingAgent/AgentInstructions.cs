

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace CodingAgent;

public class AgentInstructions : IAgentInstructions
{
    private readonly Kernel _kernel;

    public AgentInstructions(Kernel kernel)
    {
        _kernel = kernel;
    }

    public async Task InjectAsync(ChatHistory chatHistory)
    {
        var systemPrompt = await ReadSystemInstructionsAsync();
        chatHistory.Insert(0, new ChatMessageContent(AuthorRole.System, systemPrompt));
    }

    public void Remove(ChatHistory chatHistory)
    {
        if (chatHistory.Count > 0 && chatHistory[0].Role == AuthorRole.System)
        {
            chatHistory.RemoveAt(0);
        }
    }

    private async Task<string> ReadSystemInstructionsAsync()
    {
        var resourceName = "CodingAgent.Resources.AgentSystemInstructions.txt";
        var instructions = EmbeddedResource.Read(resourceName);
        return await Task.FromResult(instructions);
    }
}