using System.Runtime.Serialization;
using Slithin.Core;

namespace Slithin.VPL.Components.ViewModels;

[DataContract(IsReference = true)]
public class TextNodeViewModel : BaseViewModel
{
    private string? _label;

    private bool _previewMode;
    private string? _text;

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public string? Label
    {
        get => _label;
        set => SetValue(ref _label, value);
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public bool PreviewMode
    {
        get => _previewMode;
        set => SetValue(ref _previewMode, value);
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public string? Text
    {
        get => _text;
        set => SetValue(ref _text, value);
    }
}
