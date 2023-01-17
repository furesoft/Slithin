using Newtonsoft.Json;
using Slithin.Modules.I18N.Models;

namespace Slithin.Modules.I18N;

internal class LocalisationServiceImpl : ILocalisationService
{
    private Dictionary<string, string> _localization = new Dictionary<string, string>();

    public LocalisationServiceImpl()
    {
        var localeNames = typeof(LocalisationServiceImpl).Assembly
                                    .GetManifestResourceNames()
                                    .Select(_ => _.Replace("Slithin.Modules.I18N.Resources.Locales.", ""))
                                    .Select(_ => Path.GetFileNameWithoutExtension(_))
                                    .ToArray();

        var resourceName = localeNames.FirstOrDefault(_ => _ == Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName);
        if (resourceName == null)
        {
            resourceName = "en";
        }

        var strm = GetType().Assembly.GetManifestResourceStream($"Slithin.Modules.I18N.Resources.Locales.{resourceName}.json");
        var json = new StreamReader(strm).ReadToEnd();

        _localization = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
    }

    public string GetString(string key)
    {
        return _localization.ContainsKey(key) ? _localization[key] : "[No Value] - " + key;
    }
}
