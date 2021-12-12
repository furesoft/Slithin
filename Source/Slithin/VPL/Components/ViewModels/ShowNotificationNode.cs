using System.Runtime.Serialization;
using Slithin.VPL.Components.Views;
using Slithin.VPL.NodeBuilding;

namespace Slithin.VPL.Components.ViewModels;

[DataContract(IsReference = true)]
[NodeView(typeof(ShowNotificationView))]
public class ShowNotificationNode : VisualNode
{
    private object? _message;

    public ShowNotificationNode() : base("Show Notification")
    {
    }

    [Pin("Flow Input")]
    public IInputPin FlowInputPin { get; set; }

    [Pin("Flow Output")]
    public IOutputPin FlowOutputPin { get; set; }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public object? Message
    {
        get => _message;
        set => SetValue(ref _message, value);
    }
}
