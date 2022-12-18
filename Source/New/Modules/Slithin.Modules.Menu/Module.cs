using AuroraModularis.Core;
using Slithin.Core.Services.Implementations;
using Slithin.Modules.Menu.Models;

namespace Slithin.Modules.Menu;

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
