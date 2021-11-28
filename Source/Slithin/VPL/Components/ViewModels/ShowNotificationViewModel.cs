using System.Collections.Generic;
using System.Runtime.Serialization;
using NodeEditor.Model;
using Slithin.Core;
using Slithin.UI;
using WebAssembly;

namespace Slithin.VPL.Components.ViewModels;

[DataContract(IsReference = true)]
public class ShowNotificationViewModel : BaseViewModel, ICompilableNode
{
    private object? _label;
    private int _count;

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public object? Label
    {
        get => _label;
        set => this.SetValue(ref _label, value);
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public int Count
    {
        get => _count;
        set => this.SetValue(ref _count, value);
    }

    public INode? Parent { get; set; }
    public void Compile(Module module, List<Instruction> instructions)
    {
        throw new System.NotImplementedException();
    }
}
