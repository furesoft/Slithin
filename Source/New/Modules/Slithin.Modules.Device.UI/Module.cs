using System.Diagnostics;
using AuroraModularis.Core;
using Avalonia.Controls;
using Avalonia.Layout;
using Slithin.Controls.Ports.Extensions;
using Slithin.Modules.BaseServices.Models;
using Slithin.Modules.Device.UI.ViewModels;
using Slithin.Modules.UI.Models;

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
