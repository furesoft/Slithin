using AuroraModularis.Core;
using Slithin.Modules.Device.Models;

namespace Slithin.Modules.Device.Mock;

public class Module : AuroraModularis.Module
{
    public override Task OnStart(Container container)
    {
        return Task.CompletedTask;
    }

    public override void RegisterServices(Container container)
    {
        container.Register<IRemarkableDevice>(new MockDevice()).AsSingleton();
        container.Register<IXochitlService>(new XochitlMock()).AsSingleton();
    }
}
