﻿using AuroraModularis.Core;

namespace Slithin.Modules.MarketplaceIntegration;

public class Module : AuroraModularis.Module
{
    public override Task OnStart(Container container)
    {
        return Task.CompletedTask;
    }
}
