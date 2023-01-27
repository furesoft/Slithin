using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Layout;
using Slithin.Modules.Settings.Models.Builder.Attributes;

namespace Slithin.Modules.Settings.Builder.ControlProviders;

public class ToggleProvider : ISettingsControlProvider
{
    public Type AttributeType => typeof(ToggleAttribute);

    public Control Build(string bindingName, object settingsObj, SettingsBaseAttribute settingsBaseAttribute)
    {
        return new ToggleSwitch
        {
            [!ToggleButton.IsCheckedProperty] = new Binding(bindingName, BindingMode.TwoWay) {Source = settingsObj}
        };
    }

    public bool CanHandle(Type propType)
    {
        return propType == typeof(bool);
    }
}
