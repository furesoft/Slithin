using System.Runtime.Serialization;
using Slithin.VPL.Components.Views;
using Slithin.VPL.NodeBuilding;

namespace Slithin.VPL.Components.ViewModels;

[DataContract(IsReference = true)]
[NodeView(typeof(TextNodeView))]
public class TextNodeViewModel : NodeViewModelBase
{
    private string? _text;

    public TextNodeViewModel() : base("Text")
    {
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public string? Text
    {
        get => _text;
        set => SetValue(ref _text, value);
    }
}
