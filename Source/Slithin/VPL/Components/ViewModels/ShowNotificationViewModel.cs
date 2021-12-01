using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Slithin.Core;
using Slithin.UI;
using WebAssembly;

namespace Slithin.VPL.Components.ViewModels;

[DataContract(IsReference = true)]
public class ShowNotificationViewModel : BaseViewModel, ICompilableNode
{
    private int _count;
    private object? _label;

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public object? Label
    {
        get => _label;
        set => SetValue(ref _label, value);
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public int Count
    {
        get => _count;
        set => SetValue(ref _count, value);
    }

    public void Compile(Module module, List<Instruction> instructions)
    {
        throw new NotImplementedException();
    }
}
