using System.Runtime.Serialization;
using Slithin.Core;

namespace Slithin.VPL.Components.ViewModels;

[DataContract(IsReference = true)]
public class ExitViewModel : BaseViewModel
{
    private object? _label;

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public object? Label
    {
        get => _label;
        set => SetValue(ref _label, value);
    }
}
