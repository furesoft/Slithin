namespace Slithin.Modules.Settings.Models;

/// <summary>
/// A service to manage user settings
/// </summary>
public interface ISettingsService
{
    SettingsModel GetSettings();

    void Save(SettingsModel settings);
}
