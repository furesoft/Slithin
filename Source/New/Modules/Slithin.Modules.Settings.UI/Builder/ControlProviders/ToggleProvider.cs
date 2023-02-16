using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Slithin.Modules.Settings.Models.Builder;
using Slithin.Modules.Settings.Models.Builder.Attributes;

namespace Slithin.Modules.Settings.UI.Builder.ControlProviders;

public class ToggleProvider : ISettingsControlProvider
{
    public Type AttributeType => typeof(ToggleAttribute);

    public Control Build(string bindingName, object settingsObj, SettingsAttribute settingsAttribute)
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

    public bool HideLabel { get; }
}
