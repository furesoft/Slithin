using System.Runtime.Serialization;
using Slithin.VPL.NodeBuilding;

namespace Slithin.VPL.Components.ViewModels;

[DataContract(IsReference = true)]
[NodeCategory("Value")]
public class LoadSettingNode : VisualNode
{
    public LoadSettingNode() : base("Load Setting")
    {
    }

    [Pin("Key")]
    public IInputPin KeyPin { get; set; }

    [Pin("Output")]
    public IOutputPin OutputPin { get; set; }
}
