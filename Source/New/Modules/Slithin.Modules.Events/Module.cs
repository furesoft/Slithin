using AuroraModularis.Core;
using Slithin.Models.Events;

namespace Slithin.Modules.I18N;

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
