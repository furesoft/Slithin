using AuroraModularis.Core;

namespace Slithin.Modules.Backup;

internal class Module : AuroraModularis.Module
{
    public override Task OnStart(ServiceContainer container)
    {
        return Task.CompletedTask;
    }
}
