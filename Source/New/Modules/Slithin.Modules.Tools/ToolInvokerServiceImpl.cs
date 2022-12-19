using Slithin.Modules.Tools.Models;

namespace Slithin.Core.Tools;

public class ToolInvokerServiceImpl : IToolInvokerService
{
    private static readonly Dictionary<string, ITool> _tools = new();

    public Dictionary<string, ITool> Tools => _tools;

    public void Init()
    {
        foreach (var tool in Utils.Find<ITool>())
        {
            Tools.Add(tool.Info.ID, tool);
        }
    }

    public void Invoke(string id, ToolProperties props)
    {
        if (Tools.ContainsKey(id))
        {
            Tools[id].Invoke(props);
        }
    }
}
