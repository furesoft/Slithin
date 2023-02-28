using AuroraModularis.Core;
using Slithin.Modules.Sync.Models;

namespace Slithin.Modules.Sync;

public class Module : AuroraModularis.Module
{
    public override Task OnStart(ServiceContainer container)
    {
        return Task.CompletedTask;
    }

    public override void RegisterServices(ServiceContainer container)
    {
        container.Register<NotebooksFilter>().AsSingleton();
        container.Register<TemplatesFilter>().AsSingleton();
        container.Register<ISynchronizeService>(new SynchronizeImpl()).AsSingleton();
    }
}
