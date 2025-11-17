using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using Microsoft.SemanticKernel;

namespace CodingAgent.Plugins.Shell;

public class UnixShellPlugin
{
    [KernelFunction("shell")]
    [Description(
        """
        You can use the shell tool to execute any command. It can be used to solve a wide range of problems.
        
        **Important:** Only use ripgrep - `rg` - for searching through files. Other solutions produce output that's too big to handle.
        Use `rg --files | rg <filename>` to locate files. Use `rg <regex> -l` to search for specific patterns in files.
        
        Chain multiple commands using `&&` and avoid newlines in the command. For example `cd example && rg MyClass`.)]
        """)]
    public async Task<string> ExecuteCommandAsync(string command)
    {
        var environmentVariables = new Dictionary<string, string?>
        {
            ["EDITOR"] = "sh -c 'echo \\\"Interactive editor not available in this environment.\\\" >&2; exit 1'",
            ["VISUAL"] = "sh -c 'echo \\\"Interactive editor not available in this environment.\\\" >&2; exit 1'",
            ["GIT_PAGER"] = "cat",
            ["GIT_TERMINAL_PROMPT"] = "0",
            ["GIT_SEQUENCE_EDITOR"] = "sh -c 'echo \\\"Interactive Git commands are not supported in this environment.\\\" >&2; exit 1'",
            ["GIT_EDITOR"] = "sh -c 'echo \\\"Interactive Git commands are not supported in this environment.\\\" >&2; exit 1'",
        };

        var processStartInfo = new ProcessStartInfo
        {
            FileName = "/bin/bash",
            Arguments = $"-c \"{command}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };
        
        foreach(var (key, value) in environmentVariables)
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