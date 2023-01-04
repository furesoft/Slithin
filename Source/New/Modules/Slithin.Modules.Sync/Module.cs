using AuroraModularis.Core;
using Slithin.Modules.Sync.Models;

namespace Slithin.Modules.Sync;

public class Module : AuroraModularis.Module
{
    public override Task OnStart(Container container)
    {
        return Task.CompletedTask;
    }

    public override void RegisterServices(Container container)
    {
        container.Register<NotebooksFilter>().AsSingleton();
        container.Register<TemplatesFilter>().AsSingleton();
    }
}
