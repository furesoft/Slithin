using NodeEditor.Model;

namespace Slithin.VPL.ExecutionModel;

internal class ExecutionNode
{
    public ExecutionNode Next { get; set; }
    public INode Node { get; internal set; }
}
