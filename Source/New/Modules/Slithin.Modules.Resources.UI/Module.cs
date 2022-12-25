using AuroraModularis.Core;

namespace Slithin.Modules.Resources.UI;

internal class Module : AuroraModularis.Module
{
    public override Task OnStart(Container container)
    {
        return Task.CompletedTask;
    }

    public override void RegisterServices(Container container)
    {
        container.Register(new MarketplaceAPI());
    }
}
