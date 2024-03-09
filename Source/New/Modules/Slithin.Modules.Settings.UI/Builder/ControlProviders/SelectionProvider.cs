using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Slithin.Modules.Settings.Models.Builder;
using Slithin.Modules.Settings.Models.Builder.Attributes;

namespace Slithin.Modules.Settings.UI.Builder.ControlProviders;

public class SelectionProvider : ISettingsControlProvider
{
    public Type AttributeType => typeof(SelectionAttribute);

    public Control Build(string bindingName, object settingsObj, SettingsAttribute settingsAttribute)
    {
        return new ComboBox()
        {
            [!SelectingItemsControl.SelectedItemProperty] =
                new Binding(((SelectionAttribute)settingsAttribute).SelectionPropertyName),
            [!ItemsControl.ItemsSourceProperty] = new Binding(bindingName) {Source = settingsObj}
        };
    }

    public bool CanHandle(Type propType)
    {
        return propType == typeof(string[]);
    }

    public bool HideLabel { get; }
}
