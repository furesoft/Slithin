using AuroraModularis;
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
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new ConnectWindow();
        }

        await BootstrapperBuilder.StartConfigure()
            .WithAppName("Slithin")
            .WithModulesBasePath(".")
            .WithSettingsBasePath(".")
            .WithSettingsProvider<LiteDbSettingsProvider>()
            .BuildAndStartAsync();

        base.OnFrameworkInitializationCompleted();
    }
}
