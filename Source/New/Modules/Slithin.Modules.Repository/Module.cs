using AuroraModularis.Core;
using Slithin.Modules.Repository.Models;

namespace Slithin.Modules.Repository;

public class Module : AuroraModularis.Module
{
    public override Task OnStart(Container container)
    {
        var pathManager = container.Resolve<IPathManager>();
        pathManager.Init();

        return Task.CompletedTask;
    }

    public override void RegisterServices(Container container)
    {
        container.Register<IPathManager>(new PathManagerImpl());
        container.Register<IVersionService>(new VersionServiceImpl(container));
        container.Register<IRepository>(new LocalRepository(container));
    }
}
