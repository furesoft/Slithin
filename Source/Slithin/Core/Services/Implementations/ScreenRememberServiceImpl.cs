using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace Slithin.Core.Services.Implementations;

public class ScreenRememberServiceImpl : IScreenRememberService
{
    private readonly ISettingsService _settingsService;

    public ScreenRememberServiceImpl(ISettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    public bool HasMultipleScreens()
    {
        if (App.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            return desktop.MainWindow.Screens.All.Count > 1;
        }

        return false;
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
