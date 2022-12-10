using AuroraModularis;
using Slithin.Core.Services;
using Slithin.Core.Services.Implementations;

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
    }
}
