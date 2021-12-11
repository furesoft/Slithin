using System.Runtime.Serialization;
using Slithin.Core;

namespace Slithin.VPL;

[DataContract(IsReference = true)]
public class NodeViewModelBase : BaseViewModel
{
    private object? _label;

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public object? Label
    {
        get => _label;
        set => SetValue(ref _label, value);
    }
}
