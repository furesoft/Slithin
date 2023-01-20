using AuroraModularis.Core;
using Slithin.Modules.Settings.Models;

namespace Slithin.Modules.Settings;

internal class Module : AuroraModularis.Module
{
    public override Task OnStart(Container container)
    {
        var settingsService = container.Resolve<ISettingsService>();
        var settingsObject = settingsService.GetSettings();

        container.Register(settingsObject).AsSingleton();

        return Task.CompletedTask;
    }

    public override void RegisterServices(Container container)
    {
        var settingsService = new SettingsServiceImpl(container);

        container.Register((ISettingsService)settingsService).AsSingleton();
        container.Register<IScreenRememberService>(new ScreenRememberServiceImpl(settingsService)).AsSingleton();
    }
}
