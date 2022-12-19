using AuroraModularis.Core;
using Slithin.Core.Tools;
using Slithin.Modules.Tools.Models;

namespace Slithin.Modules.Settings;

public class Module : AuroraModularis.Module
{
    public override Task OnStart(Container container)
    {
        var toolInvoker = container.Resolve<IToolInvokerService>();
        toolInvoker.Init();

        return Task.CompletedTask;
    }

    public override void RegisterServices(Container container)
    {
        container.Register<IToolInvokerService>(new ToolInvokerServiceImpl());
    }
}
