using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using CodingAgent;
using Spectre.Console;

var configuration = new ConfigurationBuilder()
    .AddUserSecrets(typeof(Program).Assembly)
    .AddEnvironmentVariables()
    .Build();

var kernelBuilder = Kernel.CreateBuilder()
    .AddAzureOpenAIChatCompletion(
        configuration["LanguageModel:DeploymentName"]!,
        configuration["LanguageModel:Endpoint"]!,
        configuration["LanguageModel:ApiKey"]!
    );

var kernel = kernelBuilder.Build();
var instructions = new AgentInstructions();
var agent = new Agent(kernel, instructions);

AnsiConsole.Write(new Markup("[green]CODING AGENT[/]\n"));
AnsiConsole.Write(new Rule());

while (true)
{
    var prompt = AnsiConsole.Prompt(new TextPrompt<string>(">"));
    var callbacks = new ConsoleCallbacks();

    // Stop the application when the user enters /exit or /quit.
    if (string.Compare(prompt, "/exit", StringComparison.OrdinalIgnoreCase) == 0 ||
        string.Compare(prompt, "/quit", StringComparison.OrdinalIgnoreCase) == 0)
    {
        break;
    }

    await agent.InvokeAsync(prompt, callbacks);
}