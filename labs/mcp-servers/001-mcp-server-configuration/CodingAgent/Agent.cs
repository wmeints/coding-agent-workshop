using CodingAgent.Plugins.Shell;
using CodingAgent.Plugins.TaskMemory;
using CodingAgent.Plugins.TextEditor;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;

namespace CodingAgent;

public class Agent
{
    private ChatHistory _chatHistory;
    private readonly Kernel _kernel;
    private readonly IAgentInstructions _agentInstructions;
    private readonly AgentContext _context;

    public Agent(Kernel kernel, AgentContext context, IAgentInstructions agentInstructions)
    {
        _chatHistory = new ChatHistory();
        _kernel = kernel;
        _context = context;
        _agentInstructions = agentInstructions;

        _kernel.Plugins.Add(ShellPluginFactory.Create());
        _kernel.Plugins.AddFromObject(new TextEditorPlugin());
        _kernel.Plugins.AddFromObject(new TaskMemoryPlugin());
    }

    public async Task InvokeAsync(string prompt, IAgentCallbacks callbacks)
    {
        var chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();

        await _agentInstructions.InjectAsync(_kernel, _context, _chatHistory);
        _chatHistory.AddUserMessage(prompt);

        while (true)
        {
            var promptExecutionSettings = new AzureOpenAIPromptExecutionSettings()
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(autoInvoke: false),
            };

            var response = await chatCompletionService.GetChatMessageContentAsync(
                _chatHistory, promptExecutionSettings, _kernel);

            _chatHistory.Add(response);

            if (!string.IsNullOrEmpty(response.Content))
            {
                await callbacks.ReportAgentResponseAsync(response.Content);
            }

            var functionCalls = FunctionCallContent.GetFunctionCalls(response).ToList();

            if (!functionCalls.Any())
            {
                // End the agent turn when there are no more function calls to process.
                break;
            }

            await HandleFunctionCalls(functionCalls, callbacks);
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
                output);
        }
    }
}