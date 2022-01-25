﻿using System.Collections.Generic;
using System.Linq;
using Serilog;
using Slithin.Core.FeatureToggle;
using Slithin.Tools;

namespace Slithin.Core;

public class ToolInvoker
{
    private static readonly Dictionary<string, ITool> _tools = new();

    private readonly ILogger _logger;

    public ToolInvoker(ILogger logger)
    {
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

        if (!Feature<Features.ToolsFeature>.IsEnabled)
        {
            return;
        }

        _logger.Information("Loading External Tools");
        //ToDo: add loading scripttools
    }

    public void Invoke(string id, ToolProperties props)
    {
        if (Tools.ContainsKey(id))
        {
            Tools[id].Invoke(props);
        }
    }
}
