using AuroraModularis.Core;
using AuroraModularis.Logging.Models;
using Slithin.Modules.Repository.Models;
using Slithin.Validators;

namespace Slithin;

[Priority(ModulePriority.Normal)]
public class Module : AuroraModularis.Module
{
    public override Task OnStart(ServiceContainer container)
    {
        container.Resolve<ILogger>().Info("Slithin started");

        return Task.CompletedTask;
    }

    public override void RegisterServices(ServiceContainer container)
    {
        container.Register<LoginInfoValidator>();
    }

    public override void OnExit()
    {
        ServiceContainer.Current.Resolve<IDatabaseService>().Dispose();
    }
}
