using OpenAI.Batch;

namespace CodingAgent;

public class AgentContext
{
    public string WorkingDirectory { get; set; } = "";
    public string OperatingSystem { get; set; } = "Unknown";

    public static AgentContext Create(string? workingDirectory = null)
    {
        string operatingSystem = "Unknown";
        
        if (System.OperatingSystem.IsWindows())
        {
            operatingSystem = "Windows";    
        }
        else if (System.OperatingSystem.IsLinux())
        {
            operatingSystem = "Linux";
        }
        else if (System.OperatingSystem.IsMacOS())
        {
            operatingSystem = "MacOS";
        }

        return new AgentContext
        {
            WorkingDirectory = workingDirectory ?? Directory.GetCurrentDirectory(),
            OperatingSystem = operatingSystem,
        };
    }
}