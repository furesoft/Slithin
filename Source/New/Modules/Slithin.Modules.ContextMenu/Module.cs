using AuroraModularis.Core;
using Slithin.Core.Services;
using Slithin.Core.Services.Implementations;

namespace Slithin.Modules.ContextMenu;

public class Module : AuroraModularis.Module
{
    public override Task OnStart(Container container)
    {
        return Task.CompletedTask;
    }

    public override void RegisterServices(Container container)
    {
        container.Register<IContextMenuProvider>(new ContextMenuProviderImpl());
    }
}
