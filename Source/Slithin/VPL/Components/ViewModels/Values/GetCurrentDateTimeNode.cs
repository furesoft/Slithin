using System.Runtime.Serialization;
using Slithin.VPL.Components.ViewModels;
using Slithin.Core.VPLNodeBuilding;

namespace Slithin.VPL.Components.ViewModels.Values;

[DataContract(IsReference = true)]
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
