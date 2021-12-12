using System.Runtime.Serialization;
using Slithin.VPL.Components.Views;
using Slithin.VPL.NodeBuilding;

namespace Slithin.VPL.Components.ViewModels;

[DataContract(IsReference = true)]
[NodeView(typeof(EntryView))]
public class EntryViewModel : NodeViewModelBase
{
    public EntryViewModel() : base("Entry")
    {
    }
}
