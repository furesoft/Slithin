namespace Slithin.Core.Services;

public interface ISettingsService
{
    Settings GetSettings();

    void Save(Settings settings);
}
