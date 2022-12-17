using AuroraModularis.Core;
using Slithin.Modules.I18N.Models;

namespace Slithin.Modules.I18N;

[Priority(ModulePriority.High)]
public class Module : AuroraModularis.Module
{
    public override Task OnStart(Container container)
    {
        return Task.CompletedTask;
    }

    public override void RegisterServices(Container container)
    {
        var service = new LocalisationServiceImpl();

        container.Register<ILocalisationService>(service).AsSingleton();
    }
}
