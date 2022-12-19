using AuroraModularis.Core;
using Avalonia.Data;
using Avalonia.Markup.Xaml;

namespace Slithin.Modules.Settings.Models;

public class SettingExtension : MarkupExtension
{
    public SettingExtension(string key)
    {
        Key = key;
    }

    public string Key { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var settings = Container.Current.Resolve<SettingsModel>();

        var binding = new Binding(Key)
        {
            Source = settings,
            Mode = BindingMode.TwoWay
        };

        return binding;
    }
}
