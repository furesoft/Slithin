﻿using AuroraModularis.Core;
using Slithin.Modules.Peering.Models;

namespace Slithin.Modules.Peering;

[Priority]
public class Module : AuroraModularis.Module
{
    public override Task OnStart(ServiceContainer serviceContainer)
    {
        return Task.CompletedTask;
    }

    public override void RegisterServices(ServiceContainer serviceContainer)
    {
        serviceContainer.Register<IPeer>(new PeerImpl());
    }
}
