using System.Collections.Generic;
using System.Runtime.Serialization;
using Slithin.Core;
using Slithin.Models;
using Slithin.VPL.Components.Views;
using Slithin.VPL.NodeBuilding;

namespace Slithin.VPL.Components.ViewModels;

[DataContract(IsReference = true)]
[NodeView(typeof(InvokeToolView))]
[IgnoreTemplate]
public class InvokeToolNode : VisualNode, INodeFactory
{
    public InvokeToolNode(ScriptInfo scriptInfo) : base(scriptInfo?.Name)
    {
    }

    [Pin("Flow Input")]
    public IInputPin FlowInputPin { get; set; }

    [Pin("Flow Output")]
    public IOutputPin FlowOutputPin { get; set; }

    [Pin("Properties")]
    public IInputPin PropertiesPin { get; set; }

    public IEnumerable<VisualNode> Create()
    {
        foreach (var tool in ServiceLocator.Container.Resolve<ToolInvoker>().Tools)
        {
            yield return new InvokeToolNode(tool.Value.Info);
        }
    }
}
