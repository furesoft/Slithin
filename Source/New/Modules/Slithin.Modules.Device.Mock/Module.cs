using AuroraModularis;
using Slithin.Modules.Device.Models;

namespace Slithin.Modules.Device.Mock;

public class Module : AuroraModularis.Module
{
    public override Task OnStart(TinyIoCContainer container)
    {
        return Task.CompletedTask;
    }

    public override void RegisterServices(TinyIoCContainer container)
    {
        container.Register<IRemarkableDevice>(new MockDevice());
        container.Register<IXochitlService>(new XochitlMock());
    }
}
