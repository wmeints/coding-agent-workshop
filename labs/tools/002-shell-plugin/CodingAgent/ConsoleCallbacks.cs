using Microsoft.SemanticKernel;
using Spectre.Console;

namespace CodingAgent;

public class ConsoleCallbacks : IAgentCallbacks
{
    public Task ReportFunctionCallAsync(string functionName, KernelArguments? arguments, object? output)
    {
        AnsiConsole.Write(new Rule($"Tool Call ({functionName})"));

        if(arguments is not null) 
        {
            foreach (var argument in arguments)
            {
                AnsiConsole.Write(new Markup($"[green]{argument.Key}:[/] {argument.Value}\n"));
            }
        }

        return Task.CompletedTask;
    }

    public Task ReportAgentResponseAsync(string responseContent)
    {
        AnsiConsole.Write(new Rule("Response"));
        AnsiConsole.Write(new Text($"{responseContent}\n"));

        return Task.CompletedTask;
    }
}