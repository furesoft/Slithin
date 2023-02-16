using Avalonia.Controls;
using Slithin.Modules.Settings.Models.Builder;
using Slithin.Modules.Settings.Models.Builder.Attributes;

namespace Slithin.Modules.Settings.UI.Builder.ControlProviders;

public class TimeProvider : ISettingsControlProvider
{
    public Type AttributeType => typeof(SettingsAttribute);
    public Control Build(string bindingName, object settingsObj, SettingsAttribute settingsAttribute)
    {
        return new TimePicker();
    }

    public bool CanHandle(Type propType)
    {
        return propType == typeof(TimeOnly);
    }

    public bool HideLabel { get; }
}
