using AuroraModularis;
using Slithin.Modules.Device.Models;

namespace Slithin.Modules.Device;

internal class Module : AuroraModularis.Module
{
    public override Task OnStart(TinyIoCContainer container)
    {
        return Task.CompletedTask;
    }

    public override void RegisterServices(TinyIoCContainer container)
    {
        container.Register<IRemarkableDevice>(new DeviceImplementation());
    }
}
