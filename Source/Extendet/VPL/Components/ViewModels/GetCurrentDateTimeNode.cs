using System.Runtime.Serialization;
using Slithin.VPL.Components.Views;
using Slithin.VPL.NodeBuilding;

namespace Slithin.VPL.Components.ViewModels;

[DataContract(IsReference = true)]
[NodeView(typeof(GetCurrentDateTimeView))]
[NodeCategory("Value")]
public class GetCurrentDateTimeNode : VisualNode
{
    public GetCurrentDateTimeNode() : base("CurrentDateTime")
    {
    }

    [Pin("Date")]
    public IInputPin DatePin { get; set; }

    [Pin("Flow Input")]
    public IInputPin FlowInputPin { get; set; }

    [Pin("Flow Output")]
    public IOutputPin FlowPin { get; set; }

    [Pin("Time")]
    public IInputPin TimePin { get; set; }
}
