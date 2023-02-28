using AuroraModularis.Core;
using Slithin.Modules.Menu.Models;
using Slithin.Modules.Menu.Models.ContextualMenu;

namespace Slithin.Modules.Menu;

[Priority(ModulePriority.Normal)]
internal class Module : AuroraModularis.Module
{
    public override Task OnStart(ServiceContainer container)
    {
        var contextMenuProvider = container.Resolve<IContextMenuProvider>();
        contextMenuProvider.Init();

        var builder = container.Resolve<IContextualMenuBuilder>();
        builder.Init();

        return Task.CompletedTask;
    }

    public override void RegisterServices(ServiceContainer container)
    {
        container.Register<IContextMenuProvider>(new ContextMenuProviderImpl()).AsSingleton();
        container.Register<IContextualMenuBuilder>(new ContextualMenuBuilderImpl()).AsSingleton();
    }
}
