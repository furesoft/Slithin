using AuroraModularis.Core;

namespace Slithin.Modules.Backup;

public class Module : AuroraModularis.Module
{
    public override Task OnStart(Container container)
    {
        return Task.CompletedTask;
    }
}
