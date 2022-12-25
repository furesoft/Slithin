using AuroraModularis.Core;
using Slithin.Modules.Settings.Models;

namespace Slithin.Modules.FirstStart;

public class Module : AuroraModularis.Module
{
    public override Task OnStart(Container container)
    {
        var settingsService = container.Resolve<ISettingsService>();
        var settings = settingsService.GetSettings();

        if (settings.IsFirstStart)
        {
            var window = new FirstStartWindow();
            window.Show();
        }

        return Task.CompletedTask;
    }
}
