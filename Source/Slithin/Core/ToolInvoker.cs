using System.Collections.Generic;
using System.Linq;
using Slithin.Core.Scripting;
using Slithin.ModuleSystem;
using Slithin.Tools;

namespace Slithin.Core;

public class ToolInvoker
{
    private static readonly Dictionary<string, ITool> _tools = new();
    private readonly Automation _automation;

    public ToolInvoker(Automation automation)
    {
        _automation = automation;
    }

    public Dictionary<string, ITool> Tools => _tools;

    public void Init()
    {
        foreach (var tool in Utils.Find<ITool>().Where(_ => _ is not ScriptTool))
        {
            Tools.Add(tool.Info.ID, tool);
        }

        foreach (var tool in _automation.Modules)
        {
            var info = ActionModule.GetScriptInfo(tool);


            var scriptTool = new ScriptTool(info, tool);
            scriptTool.Init();

            Tools.Add(info.Id, scriptTool);
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
