using System.Runtime.Serialization;
using Slithin.VPL.Components.ViewModels;
using Slithin.Core.VPLNodeBuilding;

namespace Slithin.VPL.Components.ViewModels.ControlFlow;

[DataContract(IsReference = true)]
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
