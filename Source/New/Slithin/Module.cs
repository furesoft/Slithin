using AuroraModularis.Core;
using AuroraModularis.Logging.Models;
using Slithin.Validators;

namespace Slithin;

[Priority(ModulePriority.Normal)]
public class Module : AuroraModularis.Module
{
    public override Task OnStart(Container container)
    {
        container.Resolve<ILogger>().Info("Slithin started");

        return Task.CompletedTask;
    }

    public override void RegisterServices(Container container)
    {
        container.Register<LoginInfoValidator>();
    }
}
