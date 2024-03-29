﻿using AuroraModularis.Core;
using Slithin.Modules.Tools.UI.ViewModels;

namespace Slithin.Modules.Tools.UI;

[Priority(ModulePriority.Max)]
internal class Module : AuroraModularis.Module
{
    public override Task OnStart(ServiceContainer container)
    {
        return Task.CompletedTask;
    }

    public override void RegisterServices(ServiceContainer container)
    {
        container.Register<ToolsPageViewModel>();
    }
}
