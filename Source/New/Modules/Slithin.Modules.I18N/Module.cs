using AuroraModularis;
using AuroraModularis.Core;

namespace Slithin.Modules.I18N;

[Priority(ModulePriority.High)]
public class Module : AuroraModularis.Module
{
    public override Task OnStart(TinyIoCContainer container)
    {
        return Task.CompletedTask;
    }

    public override void RegisterServices(TinyIoCContainer container)
    {
        var service = new LocalisationServiceImpl();
        service.Init();

        container.Register<ILocalisationService>(service);
    }
}
