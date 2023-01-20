using AuroraModularis.Core;
using Slithin.Modules.Updater.Models;

namespace Slithin.Modules.Updater;

internal class Module : AuroraModularis.Module
{
    public override Task OnStart(Container container)
    {
        return Task.CompletedTask;
    }

    public override void RegisterServices(Container container)
    {
        container.Register<IUpdaterService>(new UpdaterImplementation()).AsSingleton();
    }
}
