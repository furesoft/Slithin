using System.Runtime.Serialization;
using Slithin.VPL.Components.Views;
using Slithin.VPL.NodeBuilding;

namespace Slithin.VPL.Components.ViewModels;

[DataContract(IsReference = true)]
[NodeView(typeof(ShowNotificationView))]
public class ShowNotificationViewModel : NodeViewModelBase
{
    private object? _message;

    public ShowNotificationViewModel() : base("Show Notification")
    {
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public object? Message
    {
        get => _message;
        set => SetValue(ref _message, value);
    }
}
