﻿using AuroraModularis.Core;
using Slithin.Modules.Device.UI.ViewModels;

namespace Slithin.Modules.Device.UI;

[Priority(ModulePriority.Max)]
internal class Module : AuroraModularis.Module
{
    public override Task OnStart(ServiceContainer container)
    {
        return Task.CompletedTask;
    }

    public override void RegisterServices(ServiceContainer container)
    {
        container.Register<DevicePageViewModel>();
    }
}
