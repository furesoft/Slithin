using AuroraModularis.Core;

namespace Slithin.Modules.I18N.Models;

public struct TranslatedString
{
    private TranslatedString(string key)
    {
        Key = key;
    }

    public string Key { get; set; }

    public static implicit operator TranslatedString(string src)
    {
        return new TranslatedString(src);
    }

    public static implicit operator string(TranslatedString ts)
    {
        return ts.ToString();
    }

    public override string ToString()
    {
        var translationService = ServiceContainer.Current.Resolve<ILocalisationService>();

        return translationService.GetString(Key);
    }
}
