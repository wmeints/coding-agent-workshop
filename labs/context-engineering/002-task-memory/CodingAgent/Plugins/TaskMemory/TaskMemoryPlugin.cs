using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace CodingAgent.Plugins.TaskMemory;

public class TaskMemoryPlugin
{
    private List<TodoItem> _todoItems;
    
    [KernelFunction("write_todos")]
    [Description(
        """
        Use this tool for the following scenarios:
        
        1. Multi-step tasks
        2. Complex and non-trivial tasks
        3. When the user asks you to make a TODO list
        4. When the user provides you with multiple tasks to perform
        5. The plan needs multiple revisions from the first few steps
        
        How to use this tool:
        
        - Mark tasks as 'InProgress' when you start working on them.
        - Mark tasks as 'Completed' immediately after you finish them.
        - Remove completed tasks from the list to keep it clean.
        - Only work on one task at a time to maintain focus.
        - Important: When you create a TODO list, the first task is set to `InProgress` immediately.
        
        Task completion requirements are:
        
        - Each task must be fully completed before moving on to the next.
        - If you run in to blockers or need more information, keep the task `InProgress` and ask the user for help.
        - Do not mark a task as `Completed` if there are any remaining issues.
        
        Task break down guidelines:
        
        - Create specific actionable items.
        - Break complex tasks into smaller tasks that can be completed in one step.
        - Use clear, descriptive task names.
        """
    )]
    public void WriteTodos(List<TodoItem> todos)
    {
        _todoItems = todos;
    }

    [KernelFunction]
    [Description("Use this tool to read the current TODO list.")]
    public List<TodoItem> ReadTodos()
    {
        return _todoItems;
    }
}