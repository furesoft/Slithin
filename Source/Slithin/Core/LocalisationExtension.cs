using System;
using Slithin.Core.Services;

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
        var service = ServiceLocator.Container.Resolve<ILocalisationService>();

        return service.GetString(Key);
    }
}
