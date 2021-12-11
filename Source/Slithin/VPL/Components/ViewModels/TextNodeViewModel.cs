using System.Runtime.Serialization;

namespace Slithin.VPL.Components.ViewModels;

[DataContract(IsReference = true)]
public class TextNodeViewModel : NodeViewModelBase
{
    private string? _text;

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public string? Text
    {
        get => _text;
        set => SetValue(ref _text, value);
    }
}
