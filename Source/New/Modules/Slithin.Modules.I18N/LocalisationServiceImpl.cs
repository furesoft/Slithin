using System.Reflection;
using Newtonsoft.Json;
using Slithin.Modules.I18N;

namespace Slithin.Core.Services.Implementations;

public class LocalisationServiceImpl : ILocalisationService
{
    private Dictionary<string, string> _localization = new Dictionary<string, string>();

    public string GetString(string key)
    {
        return _localization.ContainsKey(key) ? _localization[key] : "[No Value] - " + key;
    }

    public void Init()
    {
        var localeNames = Assembly.GetExecutingAssembly()
                                    .GetManifestResourceNames()
                                    .Where(_ => _.StartsWith("Slithin.Modules.I18N.Resources.Locales."))
                                    .Select(_ => Path.GetFileNameWithoutExtension(_))
                                    .ToArray();

        string resourceName = localeNames.FirstOrDefault(_ => _ == Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName);
        if (resourceName == null)
        {
            resourceName = "en";
        }

        var strm = GetType().Assembly.GetManifestResourceStream($"Slithin.Modules.I18N.Resources.Locales.{resourceName}.json");
        var json = new StreamReader(strm).ReadToEnd();

        _localization = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
    }
}
