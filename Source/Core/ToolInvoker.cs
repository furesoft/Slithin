using System.Collections.Generic;
using Slithin.Core.Scripting;

namespace Slithin.Core
{
    public class ToolInvoker
    {
        private readonly Dictionary<string, ITool> _tools = new();

        public void Init()
        {
            foreach (var tool in Utils.Find<ITool>())
            {
                _tools.Add(tool.Info.ID, tool);
            }
        }

        public void Invoke(string id, ToolProperties props)
        {
            if (_tools.ContainsKey(id))
            {
                _tools[id].Invoke(props);
            }
        }
    }
}
