using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace CodingAgent;

public interface IAgentInstructions
{
    Task InjectAsync(Kernel kernel, AgentContext context,ChatHistory chatHistory);
    void Remove(ChatHistory chatHistory);
}