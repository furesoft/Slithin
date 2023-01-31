using AuroraModularis.Core;
using Slithin.Modules.Settings.Models;
using Slithin.Modules.Settings.Models.Builder;
using Slithin.Modules.Settings.UI.Builder;

namespace Slithin.Modules.Settings.UI;

internal class Module : AuroraModularis.Module
{
    public override Task OnStart(ServiceContainer container)
    {
        var settingsService = container.Resolve<ISettingsService>();
        var settingsObject = settingsService.GetSettings();

        container.Register(settingsObject).AsSingleton();
        
        container.Resolve<ISettingsUiBuilder>().RegisterSettingsModel<SettingModels.AppeareanceSettingsModel>();

        return Task.CompletedTask;
    }

    public override void RegisterServices(ServiceContainer container)
    {
        var settingsService = new SettingsServiceImpl(container);

        container.Register((ISettingsService)settingsService).AsSingleton();
        container.Register<IScreenRememberService>(new ScreenRememberServiceImpl(settingsService)).AsSingleton();
        container.Register<ISettingsUiBuilder>(new SettingsUIBuilderImpl()).AsSingleton();
    }
}
