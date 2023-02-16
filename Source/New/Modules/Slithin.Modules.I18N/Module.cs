using AuroraModularis.Core;
using Slithin.Modules.I18N.Models;

namespace Slithin.Modules.I18N;

[Priority(ModulePriority.High)]
internal class Module : AuroraModularis.Module
{
    public override Task OnStart(ServiceContainer container)
    {
        return Task.CompletedTask;
    }

    public override void RegisterServices(ServiceContainer container)
    {
        var service = new LocalisationServiceImpl();

        container.Register<ILocalisationService>(service).AsSingleton();
    }
}
