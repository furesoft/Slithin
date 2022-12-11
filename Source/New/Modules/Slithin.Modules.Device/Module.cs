using AuroraModularis;
using Slithin.Modules.Device.Models;

namespace Slithin.Modules.Device;

internal class Module : AuroraModularis.Module
{
    public override Task OnStart(TinyIoCContainer container)
    {
        var xochitl = container.Resolve<IXochitlService>();
        //xochitl.Init();

        return Task.CompletedTask;
    }

    public override void RegisterServices(TinyIoCContainer container)
    {
        container.Register<IRemarkableDevice>(new DeviceImplementation());
        container.Register<IXochitlService>(new XochitlImpl(container));
    }
}
