using AuroraModularis.Core;
using Slithin.Core.Tools;
using Slithin.Modules.Tools.Models;

namespace Slithin.Modules.Settings;

internal class Module : AuroraModularis.Module
{
    public override Task OnStart(Container container)
    {
        return Task.CompletedTask;
    }

    public override void RegisterServices(Container container)
    {
        container.Register<IToolInvokerService>(new ToolInvokerServiceImpl());
    }
}
