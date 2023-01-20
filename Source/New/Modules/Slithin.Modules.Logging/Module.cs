using AuroraModularis.Core;

namespace Slithin.Modules.Logging;

[Priority]
public class Module : AuroraModularis.Module
{
    public override Task OnStart(Container container)
    {
        return Task.CompletedTask;
    }

    public override void RegisterServices(Container container)
    {
        container.Register<AuroraModularis.Logging.Models.ILogger>(new LoggerImpl(container)).AsSingleton();
    }
}
