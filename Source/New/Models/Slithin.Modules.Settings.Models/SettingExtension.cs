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
        var settingsService = Container.Current.Resolve<ISettingsService>();

        var binding = new Binding(Key);
        binding.Source = settingsService.GetSettings();
        binding.Mode = BindingMode.TwoWay;

        return binding;
    }
}
