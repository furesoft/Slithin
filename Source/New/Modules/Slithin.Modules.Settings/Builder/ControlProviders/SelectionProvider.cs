using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Slithin.Modules.Settings.Models.Builder.Attributes;

namespace Slithin.Modules.Settings.Builder.ControlProviders;

public class SelectionProvider : ISettingsControlProvider
{
    public Type AttributeType => typeof(SelectionAttribute);

    public Control Build(string bindingName, object settingsObj, SettingsBaseAttribute settingsBaseAttribute)
    {
        return new ComboBox()
        {
            [!SelectingItemsControl.SelectedItemProperty] =
                new Binding(((SelectionAttribute)settingsBaseAttribute).SelectionPropertyName)
                {
                    Mode = BindingMode.TwoWay,
                    Source = settingsObj
                },
            [!ItemsControl.ItemsProperty] = new Binding(bindingName) { Source = settingsObj }
        };
    }

    public bool CanHandle(Type propType)
    {
        return propType == typeof(string[]);
    }
}
