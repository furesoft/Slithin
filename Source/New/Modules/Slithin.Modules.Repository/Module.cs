using AuroraModularis.Core;
using Slithin.Modules.Repository.Models;

namespace Slithin.Modules.Repository;

internal class Module : AuroraModularis.Module
{
    public override Task OnStart(Container container)
    {
        var pathManager = container.Resolve<IPathManager>();
        pathManager.Init();

        var loginService = container.Resolve<ILoginService>();
        loginService.Init();

        return Task.CompletedTask;
    }

    public override void RegisterServices(Container container)
    {
        container.Register<IPathManager>(new PathManagerImpl());
        container.Register<IVersionService>(new VersionServiceImpl(container));
        container.Register<IRepository>(new LocalRepository(container));
        container.Register<ILoginService>(new LoginServiceImpl(container));
        container.Register<IDatabaseService>(new DatabaseServiceImpl(container));
    }
}
