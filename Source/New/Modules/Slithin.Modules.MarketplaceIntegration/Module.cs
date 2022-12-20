using AuroraModularis.Core;

namespace Slithin.Modules.MarketplaceIntegration;

internal class Module : AuroraModularis.Module
{
    public override Task OnStart(Container container)
    {
        return Task.CompletedTask;
    }
}
