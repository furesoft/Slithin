using System.Runtime.Serialization;
using Slithin.Core.VPLNodeBuilding;

namespace Slithin.VPL.Components.ViewModels;

[DataContract(IsReference = true)]
[IgnoreTemplate]
public class ExitNode : VisualNode
{
    public ExitNode() : base("Exit")
    {
    }

    [Pin("Flow Input")]
    public IInputPin FlowPin { get; set; }
}
