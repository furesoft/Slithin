using System.Collections.Generic;
using System.Runtime.Serialization;
using Slithin.Core;
using Slithin.Models;
using Slithin.VPL.Components.ViewModels.FactoryNodes;
using Slithin.VPL.Components.ViewModels;
using Slithin.Core.VPLNodeBuilding;

namespace Slithin.VPL.Components.ViewModels.FactoryNodes;

[DataContract(IsReference = true)]
[NodeCategory("Tools")]
[IgnoreTemplate]
public class InvokeToolNode : VisualNode, INodeFactory
{
    public InvokeToolNode(ScriptInfo scriptInfo) : base(scriptInfo?.Name)
    {
    }

    public InvokeToolNode() : base(null)
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
