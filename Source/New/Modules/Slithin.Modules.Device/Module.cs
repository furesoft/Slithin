using AuroraModularis.Core;
using Slithin.Modules.Device.Models;

namespace Slithin.Modules.Device;

internal class Module : AuroraModularis.Module
{
    public override Task OnStart(ServiceContainer container)
    {
        var xochitl = container.Resolve<IXochitlService>();
        //xochitl.Init();

        return Task.CompletedTask;
    }

    public override void RegisterServices(ServiceContainer container)
    {
        container.Register<IRemarkableDevice>(new DeviceImplementation()).AsSingleton();
        container.Register<IXochitlService>(new XochitlImpl(container)).AsSingleton();
        container.Register<PathList>(new PathList()).AsSingleton();
    }
}
