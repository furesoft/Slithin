using System.Collections.Generic;
using System.Runtime.Serialization;
using Slithin.VPL.NodeBuilding;
using Slithin.VPL.Components.ViewModels.FactoryNodes;
using Slithin.VPL.Components.ViewModels;

namespace Slithin.VPL.Components.ViewModels.FactoryNodes;

[DataContract(IsReference = true)]
[NodeCategory("Events")]
[IgnoreTemplate]
public class EventNode : VisualNode, INodeFactory
{
    public EventNode(string name) : base(name)
    {
    }

    public EventNode() : base("")
    {
    }

    [Pin("Flow Output")]
    public IOutputPin FlowOutputPin { get; set; }

    public IEnumerable<VisualNode> Create()
    {
        yield return new EventNode("OnConnect");
    }
}
