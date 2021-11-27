using System.Runtime.Serialization;
using ReactiveUI;
using Slithin.Core;

namespace Slithin.VPL.Components.ViewModels
{
    [DataContract(IsReference = true)]
    public class OrGateViewModel : BaseViewModel
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
    }
}
