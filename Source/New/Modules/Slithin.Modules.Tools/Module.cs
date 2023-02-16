using AuroraModularis.Core;
using Slithin.Modules.Tools.Models;

namespace Slithin.Modules.Tools;

internal class Module : AuroraModularis.Module
{
    public override Task OnStart(ServiceContainer container)
    {
        return Task.CompletedTask;
    }

    public override void RegisterServices(ServiceContainer container)
    {
        container.Register<IToolInvokerService>(new ToolInvokerServiceImpl()).AsSingleton();
    }
}
