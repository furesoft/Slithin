using AuroraModularis.Core;
using Slithin.Modules.Updater.Models;

namespace Slithin.Modules.Updater;

internal class Module : AuroraModularis.Module
{
    public override Task OnStart(ServiceContainer container)
    {
        return Task.CompletedTask;
    }

    public override void OnInit()
    {
    }

    public override void RegisterServices(ServiceContainer container)
    {
        container.Register<IUpdaterService>(new UpdaterImplementation()).AsSingleton();
    }
}
