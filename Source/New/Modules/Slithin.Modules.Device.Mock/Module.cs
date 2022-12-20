using AuroraModularis.Core;
using Slithin.Modules.Device.Models;

namespace Slithin.Modules.Device.Mock;

public class Module : AuroraModularis.Module
{
    public override Task OnStart(Container container)
    {
        //Todo: copy device.zip to bin directory if not exits

        return Task.CompletedTask;
    }

    public override void RegisterServices(Container container)
    {
        container.Register<IRemarkableDevice>(new MockDevice(container)).AsSingleton();
        container.Register<IXochitlService>(new XochitlImpl(container)).AsSingleton();
        container.Register<PathList>(new PathList()).AsSingleton();
    }
}
