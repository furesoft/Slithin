using System.Runtime.Serialization;

namespace Slithin.VPL.Components.ViewModels;

[DataContract(IsReference = true)]
public class ShowNotificationViewModel : NodeViewModelBase
{
    private object? _message;

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public object? Message
    {
        get => _message;
        set => SetValue(ref _message, value);
    }
}
