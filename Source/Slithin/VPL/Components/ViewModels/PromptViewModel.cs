using System.Runtime.Serialization;

namespace Slithin.VPL.Components.ViewModels;

[DataContract(IsReference = true)]
public class PromptViewModel : NodeViewModelBase
{
    private object? _header;
    private object? _value;

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public object? Header
    {
        get => _header;
        set => SetValue(ref _header, value);
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public object? Value
    {
        get => _value;
        set => SetValue(ref _value, value);
    }
}
