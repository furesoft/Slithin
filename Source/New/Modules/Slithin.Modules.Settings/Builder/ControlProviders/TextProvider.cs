using Avalonia.Controls;
using Avalonia.Data;
using Slithin.Modules.Settings.Models.Builder;
using Slithin.Modules.Settings.Models.Builder.Attributes;

namespace Slithin.Modules.Settings.Builder.ControlProviders;

public class TextProvider : ISettingsControlProvider
{
    public Type AttributeType => typeof(SettingsAttribute);

    public Control Build(string bindingName, object settingsObj, SettingsAttribute settingsAttribute)
    {
        var textBox = new TextBox();
        
        var binding = new Binding {Path = bindingName, Mode = BindingMode.TwoWay};

        textBox[!TextBox.TextProperty] = binding;
        
        return textBox;
    }

    public bool CanHandle(Type propType)
    {
        return propType == typeof(string);
    }

    public bool HideLabel { get; }
}
