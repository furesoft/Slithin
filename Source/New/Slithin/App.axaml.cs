using AuroraModularis;
using AuroraModularis.Hooks.ResourceRegistrationHook;
using AuroraModularis.Settings.LiteDb;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Slithin.Views;

namespace Slithin;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        var bootstrapper = BootstrapperBuilder.StartConfigure()
            .WithAppName("Slithin")
            .AddResourceHook(this)
            .UseLiteDb();

        await bootstrapper.BuildAndStartAsync();

        Core.FeatureToggle.Features.Collect();
        Core.FeatureToggle.Features.EnableAll();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new ConnectWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
