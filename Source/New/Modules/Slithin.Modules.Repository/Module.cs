using AuroraModularis;
using Slithin.Core.Services;
using Slithin.Core.Services.Implementations;
using Slithin.Core.Sync;
using Slithin.Core.Sync.Repositorys;

namespace Slithin.Modules.Repository;

public class Module : AuroraModularis.Module
{
    public override Task OnStart(TinyIoCContainer container)
    {
        return Task.CompletedTask;
    }

    public override void RegisterServices(TinyIoCContainer container)
    {
        container.Register<IPathManager>(new PathManagerImpl());
        container.Register<IVersionService>(new VersionServiceImpl(container));
        container.Register<IRepository>(new LocalRepository(container));
    }
}
