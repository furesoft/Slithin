namespace Slithin.Modules.I18N.Models;

/// <summary>
/// A service to get translations for strings
/// </summary>
public interface ILocalisationService
{
    string GetString(string key);

    public string GetStringFormat(string key, params object[] args)
    {
        return string.Format(GetString(key), args);
    }
}
