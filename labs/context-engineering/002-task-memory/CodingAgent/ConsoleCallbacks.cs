using System.Text.Json;
using CodingAgent.Plugins.TaskMemory;
using Microsoft.SemanticKernel;
using Spectre.Console;

namespace CodingAgent;

public class ConsoleCallbacks : IAgentCallbacks
{
    public Task ReportFunctionCallAsync(string functionName, KernelArguments? arguments, object? output)
    {
        if (functionName == "write_todos")
        {
            ReportTodoItemListCall(JsonSerializer.Deserialize<List<TodoItem>>(arguments!["todos"].ToString()));
            return Task.CompletedTask;
        }
        
        AnsiConsole.Write(new Rule($"Tool Call ({functionName})"));

        if(arguments is not null) 
        {
            foreach (var argument in arguments)
            {
                var argumentValue = argument.Value?.ToString() ?? "";
                AnsiConsole.Write(new Markup($"[green]{argument.Key}:[/] {Markup.Escape(argumentValue)}\n"));
            }
        }

        return Task.CompletedTask;
    }

    private void ReportTodoItemListCall(List<TodoItem> todoItems)
    {
        AnsiConsole.Write("Current plan:\n");
        
        foreach (var todoItem in todoItems)
        {
            if(todoItem.Status.ToLower() == "completed")
            {
                AnsiConsole.Write($"- [x] {todoItem.Content}\n");
            }
            else if (todoItem.Status.ToLower() == "in_progress")
            {
                AnsiConsole.Write(new Markup("[bold]" + Markup.Escape($"- [ ] {todoItem.Content}") + "[/]\n"));
            }
            else
            {
                AnsiConsole.Write($"- [ ] {todoItem.Content}\n");
            }
        }
    }

    public Task ReportAgentResponseAsync(string responseContent)
    {
        AnsiConsole.Write(new Rule("Response"));
        AnsiConsole.Write(new Text($"{responseContent}\n"));

        return Task.CompletedTask;
    }
}