using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Slithin.Modules.Settings.Models.Builder;
using Slithin.Modules.Settings.Models.Builder.Attributes;

namespace Slithin.Modules.Settings.Builder.ControlProviders;

//ToDo: add ability to use same attribute type for different property types
public class EnumProvider : ISettingsControlProvider
{
    public Type AttributeType => typeof(SelectionAttribute);

    public Control Build(string bindingName, object settingsObj, SettingsAttribute settingsAttribute)
    {
        return new ComboBox()
        {
            [!SelectingItemsControl.SelectedItemProperty] =
                new Binding(((SelectionAttribute)settingsAttribute).SelectionPropertyName)
                {
                    Mode = BindingMode.TwoWay, Source = settingsObj
                },
            Items = Enum.GetValues(settingsObj.GetType().GetProperty(bindingName).GetValue(settingsObj).GetType())
        };
    }

    public bool CanHandle(Type propType)
    {
        return propType.IsAssignableTo(typeof(Enum));
    }

    public bool HideLabel { get; }
}
