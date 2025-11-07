using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace CodingAgent.Plugins;

public class SharedToolsPlugin
{
    public bool TaskCompleted { get; private set; }
    public string FinalOutput { get; private set; } = "";

    public void Reset()
    {
        TaskCompleted = false;
        FinalOutput = "";
    }
    
    [KernelFunction("final_output")]
    [Description(
        """
        Use this tool to provide the final answer to the user.
        The final output tool MUST be called with final answer to the user.
        """
    )]
    public void FinalToolOutput([Description("The final answer to the user.")] string output)
    {
        FinalOutput = output;
        TaskCompleted = true;
    }
}