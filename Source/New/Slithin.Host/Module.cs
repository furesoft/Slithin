using System;
using System.Threading.Tasks;
using AuroraModularis;
using AuroraModularis.Logging.Models;
using Avalonia;

namespace Slithin.Host;

public class Module : AuroraModularis.Module
{
    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace();

    public override Task OnStart(TinyIoCContainer container)
    {
        container.Resolve<ILogger>().Info("Slithin started");

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(Environment.GetCommandLineArgs());

        return Task.CompletedTask;
    }
}
