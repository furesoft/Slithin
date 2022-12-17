using AuroraModularis.Core;
using Slitnin.Modules.Cache.Models;

namespace Slithin.Modules.Caching;

public class Module : AuroraModularis.Module
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
