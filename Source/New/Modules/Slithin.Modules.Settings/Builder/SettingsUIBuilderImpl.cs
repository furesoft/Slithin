using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.Layout;
using Slithin.Controls.Settings;
using Slithin.Modules.Settings.Builder.ControlProviders;
using Slithin.Modules.Settings.Models.Builder;
using Slithin.Modules.Settings.Models.Builder.Attributes;

namespace Slithin.Modules.Settings.Builder;

public class SettingsUIBuilderImpl : ISettingsUiBuilder
{
    private readonly Dictionary<Type, Type> _providers = new()
    {
        [typeof(ToggleAttribute)] = typeof(ToggleProvider), [typeof(SelectionAttribute)] = typeof(SelectionProvider)
    };

    public void RegisterControlProvider<TAttr, TProvider>()
    {
        if (!_providers.TryAdd(typeof(TAttr), typeof(TProvider)))
        {
            _providers[typeof(TAttr)] = typeof(TProvider);
        }
    }

    public Control BuildSection(INotifyPropertyChanged settingsObject,
        [CallerArgumentExpression(nameof(settingsObject))]
        string settingsExpr = null)
    {
        var settingsObjType = settingsObject.GetType();
        var displayAttribute = settingsObjType.GetCustomAttribute<DisplaySettingsAttribute>();

        if (displayAttribute == null)
        {
            throw new InvalidOperationException(
                $"'{settingsExpr}' of type {settingsObjType} has to be decorated with the DisplaySettingsAttribute");
        }

        return new SettingsGroup
        {
            Header = displayAttribute.Label, Content = BuildGrid(settingsObjType, settingsObject)
        };
    }

    private object BuildGrid(Type settingsObjType, object settingsObj)
    {
        var grid = new Grid {Width = 250, HorizontalAlignment = HorizontalAlignment.Left};

        grid.ColumnDefinitions.Add(new(GridLength.Auto));
        grid.ColumnDefinitions.Add(new(GridLength.Star));

        var properties = settingsObjType.GetProperties();
        for (var index = 0; index < properties.Length; index++)
        {
            var prop = properties[index];
            var attr = prop.GetCustomAttribute<SettingsBaseAttribute>(true);

            if (attr is null)
            {
                continue;
            }

            grid.RowDefinitions.Add(new(GridLength.Auto));

            BuildLabelAndAddToGrid(attr, index, grid);

            if (!_providers.TryGetValue(attr.GetType(), out var providerType))
            {
                Debug.Write($"No Control Provider for {attr.GetType()} found");
                continue;
            }

            AddGeneratedProviderToGrid(settingsObj, providerType, attr, prop, index, grid);
        }

        return grid;
    }

    private static void BuildLabelAndAddToGrid(SettingsBaseAttribute attr, int index, Grid grid)
    {
        var label = new Label {Content = attr.Label, VerticalAlignment = VerticalAlignment.Center};
        Grid.SetColumn(label, 0);
        Grid.SetRow(label, index);
        grid.Children.Add(label);
    }

    private static void AddGeneratedProviderToGrid(object settingsObj, Type providerType, SettingsBaseAttribute attr,
        PropertyInfo prop, int index, Grid grid)
    {
        var provider = (ISettingsControlProvider) Activator.CreateInstance(providerType);

        if (attr.GetType() == provider.AttributeType && provider.CanHandle(prop.PropertyType))
        {
            var control = provider.Build(prop.Name, settingsObj, attr);

            control.VerticalAlignment = VerticalAlignment.Center;
            control.HorizontalAlignment = HorizontalAlignment.Right;

            Grid.SetColumn(control, 1);
            Grid.SetRow(control, index);
            grid.Children.Add(control);
        }
    }
}
