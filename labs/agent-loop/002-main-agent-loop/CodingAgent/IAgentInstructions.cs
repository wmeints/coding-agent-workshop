using Microsoft.SemanticKernel.ChatCompletion;

namespace CodingAgent;

public interface IAgentInstructions
{
    Task InjectAsync(ChatHistory chatHistory);
    void Remove(ChatHistory chatHistory);
}