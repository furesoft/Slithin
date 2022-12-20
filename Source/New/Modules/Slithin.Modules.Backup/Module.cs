using AuroraModularis.Core;

namespace Slithin.Modules.Backup;

internal class Module : AuroraModularis.Module
{
    public override Task OnStart(Container container)
    {
        return Task.CompletedTask;
    }
}
