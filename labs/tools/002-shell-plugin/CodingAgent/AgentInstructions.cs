

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.PromptTemplates.Handlebars;

namespace CodingAgent;

public class AgentInstructions : IAgentInstructions
{
    public async Task InjectAsync(Kernel kernel, AgentContext context, ChatHistory chatHistory)
    {
        var systemPrompt = await ReadSystemInstructionsAsync(kernel, context);
        chatHistory.Insert(0, new ChatMessageContent(AuthorRole.System, systemPrompt));
    }

    public void Remove(ChatHistory chatHistory)
    {
        if (chatHistory.Count > 0 && chatHistory[0].Role == AuthorRole.System)
        {
            chatHistory.RemoveAt(0);
        }
    }

    private async Task<string> ReadSystemInstructionsAsync(Kernel kernel, AgentContext context)
    {
        var promptTemplateConfig = new PromptTemplateConfig
        {
            Template = EmbeddedResource.Read("CodingAgent.Resources.AgentSystemInstructions.txt"),
            TemplateFormat = "handlebars"
        };

        var promptTemplateFactory = new HandlebarsPromptTemplateFactory
        {
            AllowDangerouslySetContent = true
        };
        
        var promptTemplate = promptTemplateFactory.Create(promptTemplateConfig);

        return await promptTemplate.RenderAsync(kernel, new KernelArguments
        {
            ["operating_system"] = context.OperatingSystem,
            ["working_directory"] = context.WorkingDirectory
        });
    }
}