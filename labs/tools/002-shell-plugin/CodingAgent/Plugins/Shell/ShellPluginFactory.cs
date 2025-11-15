using Microsoft.SemanticKernel;

namespace CodingAgent.Plugins.Shell;

public class ShellPluginFactory
{
    public static KernelPlugin Create()
    {
        if (OperatingSystem.IsWindows())
        {
            return KernelPluginFactory.CreateFromObject(new WindowsShellPlugin());
        }

        if (OperatingSystem.IsMacOS() || OperatingSystem.IsLinux())
        {
            return KernelPluginFactory.CreateFromObject(new UnixShellPlugin());
        }

        throw new NotImplementedException("This operating system is not supported.");
    }
}