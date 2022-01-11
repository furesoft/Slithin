using System.Collections.Generic;
using System.Linq;
using Serilog;
using Slithin.Core.Scripting;
using Slithin.ModuleSystem;
using Slithin.Tools;

namespace Slithin.Core;

public class ToolInvoker
{
    private static readonly Dictionary<string, ITool> _tools = new();
    private readonly Automation _automation;
    private readonly ILogger _logger;

    public ToolInvoker(Automation automation, ILogger logger)
    {
        _automation = automation;
        _logger = logger;
    }

    public Dictionary<string, ITool> Tools => _tools;

    public void Init()
    {
        _logger.Information("Loading Internal Tools");

        foreach (var tool in Utils.Find<ITool>().Where(_ => _ is not ScriptTool))
        {
            Tools.Add(tool.Info.ID, tool);
        }

        _logger.Information("Loading External Tools");
        foreach (var tool in _automation.Modules)
        {
            var info = ActionModule.GetScriptInfo(tool);

            _logger.Information($"Initialize {info.Name}");

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
