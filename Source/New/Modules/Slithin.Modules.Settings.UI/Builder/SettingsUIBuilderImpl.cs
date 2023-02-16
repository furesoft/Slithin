using System.Reflection;
using AuroraModularis.Core;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Slithin.Controls.Settings;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.Settings.Models;
using Slithin.Modules.Settings.Models.Builder;
using Slithin.Modules.Settings.Models.Builder.Attributes;
using Slithin.Modules.Settings.UI.Builder.ControlProviders;

namespace Slithin.Modules.Settings.UI.Builder;

public class SettingsUIBuilderImpl : ISettingsUiBuilder
{
    private readonly List<Type> _providers = new()
    {
        typeof(ToggleProvider), typeof(SelectionProvider), typeof(EnumProvider), typeof(TextProvider), typeof(DateTimeProvider), typeof(TimeProvider)
    };

    private readonly List<object> _settingsModels = new();

    public void RegisterControlProvider<TProvider>()
    {
        _providers.Add(typeof(TProvider));
    }

    public void RegisterSettingsModel<T>()
    {
        _settingsModels.Add(ServiceContainer.Current.Resolve<T>());
    }

    public Control Build()
    {
        var scrollViewer = new ScrollViewer
        {
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
            HorizontalScrollBarVisibility = ScrollBarVisibility.Auto
        };

        var sectionsView = new UniformGrid() { Columns = 2};

        foreach (var obj in _settingsModels)
        {
            var section = BuildSection(obj);
            sectionsView.Children.Add(section);
        }

        scrollViewer.Content = sectionsView;

        return scrollViewer;
    }

    public Control BuildSection(object settingsObject)
    {
        if (settingsObject is SavableSettingsModel model)
        {
            model.PropertyChanged += (s, e) =>
            {
                model.Save();
            };
        }
        
        var localisationService = ServiceContainer.Current.Resolve<ILocalisationService>();
        
        var settingsObjType = settingsObject.GetType();
        var displayAttribute = settingsObjType.GetCustomAttribute<DisplaySettingsAttribute>();

        if (displayAttribute == null)
        {
            throw new InvalidOperationException(
                $"Type {settingsObjType} has to be decorated with the DisplaySettingsAttribute");
        }

        return new SettingsGroup
        {
            Header = localisationService.GetString(displayAttribute.Label), Content = BuildGrid(settingsObjType, settingsObject),
            Tag = displayAttribute.IsExpanded
        };
    }

    private object BuildGrid(Type settingsObjType, object settingsObj)
    {
        var grid = new Grid {HorizontalAlignment = HorizontalAlignment.Left};

        grid.ColumnDefinitions.Add(new(GridLength.Auto));
        grid.ColumnDefinitions.Add(new(GridLength.Auto));

        var properties = settingsObjType.GetProperties();
        var index = 0;
        
        BuildProperties(settingsObj, properties, grid, index);

        return grid;
    }

    private void BuildProperties(object settingsObj, PropertyInfo[] properties, Grid grid, int index)
    {
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
                var provider = (ISettingsControlProvider) Activator.CreateInstance(providerType);

                if (provider.AttributeType != attr.GetType() || !provider.CanHandle(prop.PropertyType))
                {
                    continue;
                }

                label.IsVisible = !provider.HideLabel;

                AddGeneratedProviderToGrid(settingsObj, providerType, attr, prop, index, grid);
                index++;
            }
        }
    }


    //ToDo: Add ability to localize labels
    private static Label BuildLabelAndAddToGrid(SettingsAttribute attr, int index, Grid grid)
    {
        var localisationService = ServiceContainer.Current.Resolve<ILocalisationService>();
        
        var label = new Label { Content = localisationService.GetString(attr.Label), VerticalAlignment = VerticalAlignment.Center };
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
