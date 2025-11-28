using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using Microsoft.SemanticKernel;

namespace CodingAgent.Plugins.Shell;

public class WindowsShellPlugin
{
    [KernelFunction("shell")]
    [Description(
        """
        You can use the shell tool to execute any command that is valid in CMD. It can be used to solve a wide range of problems.
        **Important** Avoid using visual tools. If you find them necessary you're allowed to mention them to the user. 
        """)]
    public async Task<string> ExecuteCommandAsync(string command)
    {
        // Inject extra environment variables to disable various interactive tools for the agent.
        // We're also modifying the behavior slightly to make sure that paging is disabled for GIT tools.
        var environmentVariables = new Dictionary<string, string?>
        {
            ["EDITOR"] = "cmd /c \"echo Interactive editor not available in this environment. 1>&2 & exit /b 1\"",
            ["VISUAL"] = "cmd /c \"echo Interactive editor not available in this environment. 1>&2 & exit /b 1\"",
            ["GIT_PAGER"] = "type",
            ["GIT_TERMINAL_PROMPT"] = "0",
            ["GIT_SEQUENCE_EDITOR"] = "cmd /c \"echo Interactive Git commands are not supported in this environment. 1>&2 & exit /b 1\"",
            ["GIT_EDITOR"] = "cmd /c \"echo Interactive Git commands are not supported in this environment. 1>&2 & exit /b 1\"",
        };

        var processStartInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = $"/c {command}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        foreach (var (key, value) in environmentVariables)
        {
            processStartInfo.Environment[key] = value;
        }

        var process = new Process
        {
            StartInfo = processStartInfo
        };

        process.Start();

        var output = await process.StandardOutput.ReadToEndAsync();
        var error = await process.StandardError.ReadToEndAsync();

        await process.WaitForExitAsync();

        var outputBuilder = new StringBuilder();
        
        outputBuilder.AppendLine(output);

        if (!string.IsNullOrEmpty(error))
        {
            outputBuilder.AppendLine(error);
        }

        return outputBuilder.ToString();
    }
}