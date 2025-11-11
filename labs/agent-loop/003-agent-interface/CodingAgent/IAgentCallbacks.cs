using Microsoft.SemanticKernel;

namespace CodingAgent;

public interface IAgentCallbacks
{
    Task ReportFunctionCallAsync(string functionName, KernelArguments? arguments, object? output);
    Task ReportAgentResponseAsync(string responseContent);
}
