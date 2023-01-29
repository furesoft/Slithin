using AuroraModularis.Core;

namespace Slithin.Modules.Resources.UI;

internal class Module : AuroraModularis.Module
{
    public override Task OnStart(ServiceContainer container)
    {
        return Task.CompletedTask;
    }
}
