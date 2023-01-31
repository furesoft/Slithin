using System.ComponentModel;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Slithin.Controls.Settings;
using Slithin.Modules.Settings.Models.Builder;
using Slithin.Modules.Settings.Models.Builder.Attributes;
using Slithin.Modules.Settings.UI.Builder.ControlProviders;

namespace Slithin.Modules.Settings.UI.Builder;

public class SettingsUIBuilderImpl : ISettingsUiBuilder
{
    private readonly List<Type> _providers = new()
    {
        typeof(ToggleProvider), typeof(SelectionProvider), typeof(EnumProvider), typeof(TextProvider)
    };

    private readonly List<INotifyPropertyChanged> _settingsModels = new();

    public void RegisterControlProvider<TAttr, TProvider>()
    {
        _providers.Add(typeof(TProvider));
    }

    public void RegisterSettingsModel(INotifyPropertyChanged model)
    {
        _settingsModels.Add(model);
    }

    public Control Build()
    {
        var scrollViewer = new ScrollViewer
        {
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
            HorizontalScrollBarVisibility = ScrollBarVisibility.Auto
        };

        var stackPanel = new StackPanel();

        foreach (var obj in _settingsModels)
        {
            var section = BuildSection(obj);
            stackPanel.Children.Add(section);
        }

        scrollViewer.Content = stackPanel;

        return scrollViewer;
    }

    public Control BuildSection(INotifyPropertyChanged settingsObject)
    {
        var settingsObjType = settingsObject.GetType();
        var displayAttribute = settingsObjType.GetCustomAttribute<DisplaySettingsAttribute>();

        if (displayAttribute == null)
        {
            throw new InvalidOperationException(
                $"Type {settingsObjType} has to be decorated with the DisplaySettingsAttribute");
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
        var index = 0;
        foreach (var prop in properties)
        {
            var attr = prop.GetCustomAttribute<SettingsAttribute>(true);

            if (attr is null)
            {
                continue;
            }

            grid.RowDefinitions.Add(new(GridLength.Auto));

            var label = BuildLabelAndAddToGrid(attr, index, grid);

            foreach (var providerType in _providers)
            {
                var provider = (ISettingsControlProvider)Activator.CreateInstance(providerType);

                if (provider.AttributeType != attr.GetType() || !provider.CanHandle(prop.PropertyType))
                {
                    continue;
                }

                label.IsVisible = !provider.HideLabel;
                
                AddGeneratedProviderToGrid(settingsObj, providerType, attr, prop, index, grid);
                index++;
            }
        }

        return grid;
    }

    //ToDo: Add ability to localize labels
    private static Label BuildLabelAndAddToGrid(SettingsAttribute attr, int index, Grid grid)
    {
        var label = new Label {Content = attr.Label, VerticalAlignment = VerticalAlignment.Center};
        Grid.SetColumn(label, 0);
        Grid.SetRow(label, index);
        grid.Children.Add(label);

        return label;
    }

    private static void AddGeneratedProviderToGrid(object settingsObj, Type providerType, SettingsAttribute attr,
        PropertyInfo prop, int index, Grid grid)
    {
        var provider = (ISettingsControlProvider)Activator.CreateInstance(providerType);

        if (attr.GetType() == provider.AttributeType && provider.CanHandle(prop.PropertyType))
        {
            var control = provider.Build(prop.Name, settingsObj, attr);

            control.VerticalAlignment = VerticalAlignment.Center;
            control.HorizontalAlignment = HorizontalAlignment.Right;
            control.Margin = new(3);
            control.DataContext = settingsObj;

            Grid.SetColumn(control, 1);
            Grid.SetRow(control, index);
            grid.Children.Add(control);
        }
    }
}
