using Avalonia.Controls;
using Slithin.Modules.Settings.Models.Builder.Attributes;

namespace Slithin.Modules.Settings.Builder;

public interface ISettingsControlProvider
{
    public Type AttributeType { get; }
    public Control Build(string bindingName, object settingsObj, SettingsBaseAttribute settingsBaseAttribute);
    public bool CanHandle(Type propType);
}
