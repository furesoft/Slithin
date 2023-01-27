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

            var builder = new Slithin.Modules.Settings.Builder.SettingsUIBuilderImpl();
            
            builder.RegisterSettingsModel(new CustomSettings());
            builder.RegisterSettingsModel(new CustomSettings());
            builder.RegisterSettingsModel(new CustomSettings());
            
            var sections = builder.Build();

            window.Content = sections;

            desktop.MainWindow = window;
        }

        base.OnFrameworkInitializationCompleted();
    }
}