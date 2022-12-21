using AuroraModularis.Core;
using Slithin.Modules.Menu.Models;

namespace Slithin.Modules.Menu;

internal class Module : AuroraModularis.Module
{
    public override Task OnStart(Container container)
    {
        var provider = container.Resolve<IContextMenuProvider>();
        provider.Init();

        return Task.CompletedTask;
    }

    public override void RegisterServices(Container container)
    {
        container.Register<IContextMenuProvider>(new ContextMenuProviderImpl());
    }
}
