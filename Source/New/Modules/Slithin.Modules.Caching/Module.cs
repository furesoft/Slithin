using AuroraModularis;
using Slitnin.Modules.Cache.Models;

namespace Slithin.Modules.Caching;

public class Module : AuroraModularis.Module
{
    public override Task OnStart(TinyIoCContainer container)
    {
        throw new NotImplementedException();
    }

    public override void RegisterServices(TinyIoCContainer container)
    {
        container.Register<ICacheService>(new CacheServiceImpl());
    }
}
