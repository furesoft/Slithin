namespace Slithin.Modules.I18N.Models;

/// <summary>
/// A service to get translations for strings
/// </summary>
public interface ILocalisationService
{
    string GetString(string key);

    /// <summary>
    /// Automatic use string.format for the given translation key
    /// </summary>
    /// <param name="key"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public string GetStringFormat(string key, params object[] args)
    {
        return string.Format(GetString(key), args);
    }
}
