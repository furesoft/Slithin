using System.Runtime.Serialization;
using Slithin.VPL.Components.Views;
using Slithin.VPL.NodeBuilding;

namespace Slithin.VPL.Components.ViewModels;

[DataContract(IsReference = true)]
[NodeView(typeof(GetCurrentDateTimeView))]
[IgnoreTemplate]
[NodeCategory("Control")]
public class LoopNode : VisualNode
{
    public LoopNode() : base("Repeat")
    {
    }

    [Pin("Condition")]
    public IInputPin ConditionPin { get; set; }

    [Pin("Flow Input")]
    public IInputPin FlowInputPin { get; set; }

    [Pin("Flow Output")]
    public IOutputPin FlowPin { get; set; }
}
