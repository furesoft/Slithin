using AuroraModularis.Core;
using Slithin.Modules.Events.Models;

namespace Slithin.Modules.Events;

[Priority(ModulePriority.High)]
internal class Module : AuroraModularis.Module
{
    public override Task OnStart(Container container)
    {
        return Task.CompletedTask;
    }

    public override void RegisterServices(Container container)
    {
        container.Register<IEventService>(new EventServiceImpl()).AsSingleton();
    }
}
