﻿using AuroraModularis;

namespace Slithin.Modules.Initializer;

public class Module : AuroraModularis.Module
{
    public override Task OnStart(TinyIoCContainer container)
    {
        return Task.CompletedTask;
    }
}