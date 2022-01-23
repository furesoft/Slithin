using System.Runtime.Serialization;
using Slithin.VPL.Components.ViewModels;
using Slithin.Core.VPLNodeBuilding;

namespace Slithin.VPL.Components.ViewModels.Values;

[DataContract(IsReference = true)]
[NodeCategory("Value")]
public class SaveSettingNode : VisualNode
{
    public SaveSettingNode() : base("Save Setting")
    {
    }

    [Pin("Key")]
    public IInputPin KeyPin { get; set; }

    [Pin("Value")]
    public IInputPin ValuePin { get; set; }

    [Pin("Flow Output")]
    public IOutputPin FlowPin { get; set; }
}
