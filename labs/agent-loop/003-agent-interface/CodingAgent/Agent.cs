using CodingAgent.Plugins;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;

namespace CodingAgent;

public class Agent
{
    private const int MaxIterations = 50;
    private ChatHistory _chatHistory;
    private readonly Kernel _kernel;
    private readonly SharedToolsPlugin _sharedToolsPlugin;
    private readonly IAgentInstructions _agentInstructions;

    public Agent(Kernel kernel, IAgentInstructions agentInstructions)
    {
        _chatHistory = new ChatHistory();
        _kernel = kernel;
        _agentInstructions = agentInstructions;
        _sharedToolsPlugin = new SharedToolsPlugin();
        _kernel.Plugins.AddFromObject(_sharedToolsPlugin);
    }

    public async Task InvokeAsync(string prompt, IAgentCallbacks callbacks)
    {
        var iterations = 0;
        var chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();

        await _agentInstructions.InjectAsync(_chatHistory);
        _chatHistory.AddUserMessage(prompt);
        _sharedToolsPlugin.Reset();

        while (true)
        {
            iterations++;

            if (iterations > MaxIterations)
            {
                break;
            }

            var promptExecutionSettings = new AzureOpenAIPromptExecutionSettings()
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(autoInvoke: false),
            };

            var response = await chatCompletionService
                .GetChatMessageContentAsync(_chatHistory, promptExecutionSettings);

            _chatHistory.Add(response);

            var functionCalls = FunctionCallContent.GetFunctionCalls(response).ToList();

            if (functionCalls.Any())
            {
                await HandleFunctionCalls(functionCalls, callbacks);

                if (_sharedToolsPlugin.TaskCompleted)
                {
                    await callbacks.ReportTaskCompletedAsync(
                        _sharedToolsPlugin.FinalOutput);
                    
                    break;
                }
            }
            else
            {
                await callbacks.ReportAgentResponseAsync(response.Content!);
            }
        }

        _agentInstructions.Remove(_chatHistory);
    }

    private async Task HandleFunctionCalls(IEnumerable<FunctionCallContent> functionCalls, IAgentCallbacks callbacks)
    {
        foreach (var functionCall in functionCalls)
        {
            var output = await functionCall.InvokeAsync(_kernel);
            _chatHistory.Add(output.ToChatMessage());

            await callbacks.ReportFunctionCallAsync(
                functionCall.FunctionName,
                functionCall.Arguments,
                output.Result
            );
        }
    }
}