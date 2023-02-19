using AuroraModularis.Core;
using Slithin.Modules.Repository.Models;
using Slithin.Modules.Settings.Models;

namespace Slithin.Modules.FirstStart;

public class Module : AuroraModularis.Module
{
    public override Task OnStart(ServiceContainer container)
    {
        var settingsService = container.Resolve<ISettingsService>();
        var loginService = container.Resolve<ILoginService>();

        var settings = settingsService.GetSettings();

        if (!(settings.IsFirstStart && loginService.GetLoginCredentials().Any()))
        {
            return Task.CompletedTask;
        }

        var window = new FirstStartWindow();

        //window.Show();

        return Task.CompletedTask;
    }
}
