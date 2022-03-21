using System;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Slithin.ViewModels.Pages;

namespace Slithin.Core;

public class SettingExtension : MarkupExtension
{
    public SettingExtension(string key)
    {
        Key = key;
    }

    public string Key { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var viewModel = ServiceLocator.Container.Resolve<SettingsPageViewModel>();

        var binding = new Binding(Key);
        binding.Source = viewModel;
        binding.Mode = BindingMode.TwoWay;

        return binding;
    }
}
