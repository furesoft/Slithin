using AuroraModularis;
using Slithin.Modules.Updater.Models;

namespace Slithin.Modules.Updater;

public class Module : AuroraModularis.Module
{
    public override Task OnStart(TinyIoCContainer container)
    {
        return Task.CompletedTask;
    }

    public override void RegisterServices(TinyIoCContainer container)
    {
        container.Register<IUpdater>(new UpdaterImplementation(container));
    }
}
