using Avalonia.Controls;
using Slithin.Modules.Settings.Models.Builder;
using Slithin.Modules.Settings.Models.Builder.Attributes;

namespace Slithin.Modules.Settings.UI.Builder.ControlProviders;

public class DateTimeProvider : ISettingsControlProvider
{
    public Type AttributeType => typeof(SettingsAttribute);
    public Control Build(string bindingName, object settingsObj, SettingsAttribute settingsAttribute)
    {
        return new DatePicker() {  };
    }

    public bool CanHandle(Type propType)
    {
        return propType == typeof(DateTime) || propType == typeof(DateOnly);
    }

    public bool HideLabel { get; }
}
