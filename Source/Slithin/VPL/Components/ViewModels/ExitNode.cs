using System.Runtime.Serialization;
using Slithin.VPL.Components.Views;
using Slithin.VPL.NodeBuilding;

namespace Slithin.VPL.Components.ViewModels;

[DataContract(IsReference = true)]
[NodeView(typeof(ExitView))]
[IgnoreTemplate]
public class ExitNode : VisualNode
{
    public ExitNode() : base("Exit")
    {
    }

    [Pin("Flow Input")]
    public IInputPin FlowPin { get; set; }
}
