using AuroraModularis.Core;
using Slithin.Modules.Cache.Models;

namespace Slithin.Modules.Caching;

internal class Module : AuroraModularis.Module
{
    public override Task OnStart(Container container)
    {
        return Task.CompletedTask;
    }

    public override void RegisterServices(Container container)
    {
        container.Register<ICacheService>(new CacheServiceImpl()).AsSingleton();
    }
}
