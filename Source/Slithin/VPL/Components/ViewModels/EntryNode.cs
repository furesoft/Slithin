using System.Runtime.Serialization;
using Slithin.Core.VPLNodeBuilding;

namespace Slithin.VPL.Components.ViewModels;

[DataContract(IsReference = true)]
[IgnoreTemplate]
public class EntryNode : VisualNode
{
    public EntryNode() : base("Entry")
    {
    }

    [Pin("Flow Output")]
    public IOutputPin FlowPin { get; set; }
}
