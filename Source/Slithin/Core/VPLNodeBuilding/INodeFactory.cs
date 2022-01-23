using System.Collections.Generic;
using Slithin.VPL;

namespace Slithin.Core.VPLNodeBuilding
{
    public interface INodeFactory
    {
        IEnumerable<VisualNode> Create();
    }
}
