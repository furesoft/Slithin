using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace Slithin.Core.Services.Implementations;

public class ScreenRememberScreenImpl : IScreenRememberService
{
    private readonly ISettingsService _settingsService;

    public ScreenRememberScreenImpl(ISettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    public void Remember()
    {
        var settings = _settingsService.GetSettings();
        if (App.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            settings.WindowPosition = desktop.MainWindow.Bounds;
        }

        _settingsService.Save(settings);
    }

    public void Restore()
    {
        var settings = _settingsService.GetSettings();
        if (App.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow.WindowStartupLocation = WindowStartupLocation.Manual;
            desktop.MainWindow.SetValue(Window.BoundsProperty, settings.WindowPosition);
        }
    }
}
