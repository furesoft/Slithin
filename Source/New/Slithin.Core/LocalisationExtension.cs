using AuroraModularis.Core;
using Slithin.Modules.I18N.Models;

namespace Slithin.Core;

public class LocalisationExtension : Avalonia.Markup.Xaml.MarkupExtension
{
    public LocalisationExtension(string key)
    {
        Key = key;
    }

    public string Key { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var service = Container.Current.Resolve<ILocalisationService>();

        return service.GetString(Key);
    }
}
