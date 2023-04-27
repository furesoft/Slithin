using System.Runtime.Serialization;
using Slithin.VPL.Components.Views;
using Slithin.VPL.NodeBuilding;

namespace Slithin.VPL.Components.ViewModels;

[DataContract(IsReference = true)]
[NodeView(typeof(TextNodeView))]
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
