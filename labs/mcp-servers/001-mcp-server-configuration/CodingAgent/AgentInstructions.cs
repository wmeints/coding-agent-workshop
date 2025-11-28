

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.PromptTemplates.Handlebars;

namespace CodingAgent;

public class AgentInstructions : IAgentInstructions
{
    public async Task InjectAsync(Kernel kernel, AgentContext context, ChatHistory chatHistory)
    {
        var systemPrompt = await ReadSystemInstructionsAsync(kernel, context);
        var customInstructions = await ReadCustomInstructions(context);
        
        chatHistory.Insert(0, new ChatMessageContent(AuthorRole.System, systemPrompt));

        if (!string.IsNullOrEmpty(customInstructions))
        {
            chatHistory.Insert(1, new ChatMessageContent(AuthorRole.Developer, customInstructions));
        }
    }

    private async Task<string?> ReadCustomInstructions(AgentContext context)
    {
        var currentDirectory = context.WorkingDirectory;
        
        while (!string.IsNullOrEmpty(currentDirectory))
        {
            var customInstructionsPath = Path.Combine(currentDirectory, "AGENTS.md");

            if (File.Exists(customInstructionsPath))
            {
                return await File.ReadAllTextAsync(customInstructionsPath);
            }
            
            currentDirectory = Directory.GetParent(currentDirectory)?.FullName;
        }

        return null;
    }

    public void Remove(ChatHistory chatHistory)
    {
        // First, remove the system prompt from the history.
        if (chatHistory.Count > 0 && chatHistory[0].Role == AuthorRole.System)
        {
            chatHistory.RemoveAt(0);
        }
        
        // Check if there's a developer prompt with the custom instructions and remove it.
        if (chatHistory.Count > 0 && chatHistory[0].Role == AuthorRole.Developer)
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