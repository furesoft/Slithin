using System.Collections.Generic;
using System.Runtime.Serialization;
using Slithin.VPL.Components.Views;
using Slithin.VPL.NodeBuilding;

namespace Slithin.VPL.Components.ViewModels;

[DataContract(IsReference = true)]
[NodeView(typeof(InvokeToolView))]
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
