using System.Runtime.Serialization;
using Slithin.Core;

namespace Slithin.VPL;

[DataContract(IsReference = true)]
public class VisualNode : BaseViewModel
{
    private string? _label;

    public VisualNode(string label)
    {
        Label = label;
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public string? Label
    {
        get => _label;
        set => SetValue(ref _label, value);
    }
}
