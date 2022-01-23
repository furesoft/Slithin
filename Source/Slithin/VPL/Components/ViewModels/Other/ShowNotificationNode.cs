using System.Runtime.Serialization;
using Slithin.VPL.NodeBuilding;
using Slithin.VPL.Components.ViewModels;

namespace Slithin.VPL.Components.ViewModels.Other;

[DataContract(IsReference = true)]
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

    [Pin("Message")]
    public IInputPin MessagePin { get; set; }
}
