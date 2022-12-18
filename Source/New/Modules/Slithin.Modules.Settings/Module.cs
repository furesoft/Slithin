using AuroraModularis.Core;
using Slithin.Modules.Settings.Models;

namespace Slithin.Modules.Settings;

public class Module : AuroraModularis.Module
{
    public override Task OnStart(Container container)
    {
        return Task.CompletedTask;
    }

    public override void RegisterServices(Container container)
    {
        container.Register<ISettingsService>(new SettingsServiceImpl(container));
    }
}
