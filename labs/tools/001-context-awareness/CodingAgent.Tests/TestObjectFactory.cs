using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using OpenAI.Chat;
using ChatMessageContent = Microsoft.SemanticKernel.ChatMessageContent;

namespace CodingAgent.Tests;

public class TestObjectFactory
{
    public static Kernel CreateTestKernel(IChatCompletionService mockChatCompletionService)
    {
        var kernelBuilder = Kernel.CreateBuilder();
        kernelBuilder.Services.AddSingleton(mockChatCompletionService);

        return kernelBuilder.Build();
    }

    public static ChatMessageContent CreateFakeToolCall(string pluginName, string toolName, string? content = null, Dictionary<string, object?>? arguments = null)
    {
        var chatMessageParts = new ChatMessageContentItemCollection();
        
        var functionCallContent = new FunctionCallContent(
            toolName,
            pluginName,
            arguments: new KernelArguments(arguments ?? new Dictionary<string, object?>()));
        
        chatMessageParts.Add(functionCallContent);

        if (content != null)
        {
            chatMessageParts.Add(new TextContent(content));
        }
        
        return new ChatMessageContent(AuthorRole.Assistant, chatMessageParts);
    }

    public static  ChatMessageContent CreateFakeAgentResponse(string content)
    {
        return new ChatMessageContent(AuthorRole.Assistant, content);
    }
}