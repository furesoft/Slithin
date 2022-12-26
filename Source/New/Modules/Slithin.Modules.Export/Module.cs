﻿using AuroraModularis.Core;

namespace Slithin.Modules.Export;

[Priority(ModulePriority.High)]
internal class Module : AuroraModularis.Module
{
    public override Task OnStart(Container container)
    {
        return Task.CompletedTask;
    }

    public override void RegisterServices(Container container)
    {
    }
}