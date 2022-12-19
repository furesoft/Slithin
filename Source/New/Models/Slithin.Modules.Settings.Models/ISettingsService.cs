namespace Slithin.Modules.Settings.Models;

public interface ISettingsService
{
    SettingsModel GetSettings();

    void Save(SettingsModel settings);
}
