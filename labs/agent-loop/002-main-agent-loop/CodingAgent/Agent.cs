using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;

namespace CodingAgent;

public class Agent
{
  private const int MaxIterations = 50;
  private ChatHistory _chatHistory;
  private readonly Kernel _kernel;

  public Agent(Kernel kernel)
  {
    _chatHistory = new ChatHistory();
    _kernel = kernel;
  }

  public async Task InvokeAsync(string prompt, IAgentCallbacks callbacks)
  {
    var iterations = 0;
    var chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();

    _chatHistory.AddUserMessage(prompt);
    
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
      
      _chatHistory.AddUserMessage(prompt);

      var functionCalls = FunctionCallContent.GetFunctionCalls(response).ToList();

      if (functionCalls.Any())
      {
        await HandleFunctionCalls(functionCalls);
      }
    }
  }

  private async Task HandleFunctionCalls(IEnumerable<FunctionCallContent> functionCalls)
  {
    foreach (var functionCall in functionCalls)
    {
      var output = await functionCall.InvokeAsync(_kernel);
      _chatHistory.Add(output.ToChatMessage());
    }
  }
}
