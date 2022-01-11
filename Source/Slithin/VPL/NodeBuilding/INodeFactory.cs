using System.Collections.Generic;

namespace Slithin.VPL.NodeBuilding
{
    public interface INodeFactory
    {
        IEnumerable<VisualNode> Create();
    }
}
