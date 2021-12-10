using System.Runtime.Serialization;
using Slithin.Core;

namespace Slithin.VPL.Components.ViewModels;

[DataContract(IsReference = true)]
public class ShowNotificationViewModel : BaseViewModel
{
    private object? _label;

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public object? Label
    {
        get => _label;
        set => SetValue(ref _label, value);
    }

    private object? _message;

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public object? Message
    {
        get => _message;
        set => SetValue(ref _message, value);
    }
}
