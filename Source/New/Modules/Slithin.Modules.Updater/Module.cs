using AuroraModularis.Core;
using Slithin.Modules.Updater.Models;

namespace Slithin.Modules.Updater;

public class Module : AuroraModularis.Module
{
    public override Task OnStart(Container container)
    {
        return Task.CompletedTask;
    }

    public override void RegisterServices(Container container)
    {
        container.Register<IUpdater>(new UpdaterImplementation(container));
    }
}
