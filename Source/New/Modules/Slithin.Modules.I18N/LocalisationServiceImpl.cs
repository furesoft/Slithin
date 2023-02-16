using Newtonsoft.Json;
using Slithin.Modules.I18N.Models;

namespace Slithin.Modules.I18N;

public class LocalisationServiceImpl : ILocalisationService
{
    private Dictionary<string, string> _localization;

    public LocalisationServiceImpl()
    {
        var localeNames = typeof(LocalisationServiceImpl).Assembly
                                    .GetManifestResourceNames()
                                    .Select(_ => _.Replace("Slithin.Modules.I18N.Resources.Locales.", ""))
                                    .Select(Path.GetFileNameWithoutExtension);

        var resourceName = localeNames.FirstOrDefault(_ => _ == Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName) ?? "en";

        var strm = GetType().Assembly.GetManifestResourceStream($"Slithin.Modules.I18N.Resources.Locales.{resourceName}.json");
        var json = new StreamReader(strm!).ReadToEnd();

        _localization = JsonConvert.DeserializeObject<Dictionary<string, string>>(json)!;
    }

    public string GetString(string key)
    {
        return _localization.TryGetValue(key, out var value) ? value : "[No Value] - " + key;
    }
}
