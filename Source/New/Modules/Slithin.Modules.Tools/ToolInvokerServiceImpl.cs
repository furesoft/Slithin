using AuroraModularis.Core;
using Slithin.Modules.Tools.Models;

namespace Slithin.Modules.Tools;

internal class ToolInvokerServiceImpl : IToolInvokerService
{
    private static readonly Dictionary<string, ITool> _tools = new();

    public Dictionary<string, ITool> Tools => _tools;

    public void Init()
    {
        var typeFinder = ServiceContainer.Current.Resolve<ITypeFinder>();
        foreach (var tool in typeFinder.FindAndResolveTypes<ITool>())
        {
            Tools.Add(tool.Info.ID, tool);
        }
    }

    public void Invoke(string id, ToolProperties props)
    {
        if (Tools.TryGetValue(id, out var value))
        {
            value.Invoke(props);
        }
    }
}
