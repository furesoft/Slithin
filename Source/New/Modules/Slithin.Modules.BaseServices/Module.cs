using AuroraModularis.Core;
using Slithin.Modules.BaseServices.Models;

namespace Slithin.Modules.BaseServices;

[Priority(ModulePriority.High)]
internal class Module : AuroraModularis.Module
{
    public override Task OnStart(Container container)
    {
        return Task.CompletedTask;
    }

    public override void RegisterServices(Container container)
    {
        container.Register<IPathManager>(new PathManagerImpl()).AsSingleton();
        container.Register<IDeviceDiscovery>(new DeviceDiscoveryImpl()).AsSingleton();
        container.Register<ICacheService>(new CacheServiceImpl()).AsSingleton();
        container.Register<IEventService>(new EventServiceImpl()).AsSingleton();
        container.Register<AuroraModularis.Logging.Models.ILogger>(new LoggerImpl(container)).AsSingleton();
    }
}
