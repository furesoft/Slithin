using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace SettingsBuilderTestApp;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var window = new Window();

            var settings = new CustomSettings();

            var builder = new Slithin.Modules.Settings.Builder.SettingsUIBuilderImpl();
            var section = builder.Build(new[] {settings, settings, settings, settings});

            window.Content = section;

            desktop.MainWindow = window;
        }

        base.OnFrameworkInitializationCompleted();
    }
}
