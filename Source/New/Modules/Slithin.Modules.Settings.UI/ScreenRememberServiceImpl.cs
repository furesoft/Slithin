using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Slithin.Modules.Settings.Models;

namespace Slithin.Modules.Settings.UI;

public class ScreenRememberServiceImpl : IScreenRememberService
{
    private readonly ISettingsService _settingsService;

    public ScreenRememberServiceImpl(ISettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    public bool HasMultipleScreens()
    {
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            return desktop.MainWindow?.Screens.All.Count > 1;
        }

        return false;
    }

    public void Remember()
    {
        var settings = _settingsService.GetSettings();
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            settings.WindowPosition = desktop.MainWindow.Bounds;
        }

        _settingsService.Save(settings);
    }

    public void Restore()
    {
        var settings = _settingsService.GetSettings();
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow.WindowStartupLocation = WindowStartupLocation.Manual;
            desktop.MainWindow.SetValue(Avalonia.Visual.BoundsProperty, settings.WindowPosition);
        }
    }
}
