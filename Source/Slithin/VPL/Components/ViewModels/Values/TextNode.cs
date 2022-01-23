using System.Runtime.Serialization;
using Slithin.VPL.NodeBuilding;
using Slithin.VPL.Components.ViewModels;

namespace Slithin.VPL.Components.ViewModels.Values;

[DataContract(IsReference = true)]
[NodeCategory("Value")]
public class TextNode : VisualNode
{
    private string? _text;

    public TextNode() : base("Text")
    {
    }

    [Pin("Value")]
    public IOutputPin FlowOutputPin { get; set; }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public string? Text
    {
        get => _text;
        set => SetValue(ref _text, value);
    }
}
