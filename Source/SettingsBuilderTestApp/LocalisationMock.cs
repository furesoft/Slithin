using Slithin.Modules.I18N.Models;

namespace SettingsBuilderTestApp;

public class LocalisationMock : ILocalisationService
{
    public string GetString(string key)
    {
        return key;
    }
}
