using System.Runtime.Serialization;
using Slithin.VPL.Components.Views;
using Slithin.VPL.NodeBuilding;

namespace Slithin.VPL.Components.ViewModels;

[DataContract(IsReference = true)]
[NodeView(typeof(ExitView))]
public class ExitViewModel : NodeViewModelBase
{
    public ExitViewModel() : base("Exit")
    {
    }
}
