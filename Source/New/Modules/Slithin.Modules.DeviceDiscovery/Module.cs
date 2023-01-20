using AuroraModularis.Core;
using Slithin.Modules.DeviceDiscovery.Models;

namespace Slithin.Modules.DeviceDiscovery;

internal class Module : AuroraModularis.Module
{
    public override Task OnStart(Container container)
    {
        return Task.CompletedTask;
    }

    public override void RegisterServices(Container container)
    {
        container.Register<IDeviceDiscovery>(new DeviceDiscoveryImpl()).AsSingleton();
    }
}
