using System.Runtime.Serialization;
using Slithin.VPL.Components.Views;
using Slithin.VPL.NodeBuilding;

namespace Slithin.VPL.Components.ViewModels;

[DataContract(IsReference = true)]
[NodeView(typeof(GetCurrentDateTimeView))]
[NodeCategory("General")]
public class TimerNode : VisualNode
{
    public TimerNode() : base("Timer")
    {
    }


    [Pin("Flow Input")]
    public IInputPin FlowInputPin { get; set; }

    [Pin("Flow Output")]
    public IOutputPin FlowPin { get; set; }

    [Pin("Repeat")]
    public IOutputPin RepeatPin { get; set; }

    [Pin("Intervaö")]
    public IInputPin IntervalPin { get; set; }
}
