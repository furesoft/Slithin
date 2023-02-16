using AuroraModularis.Core;
using Slithin.Modules.UI.Implementations;
using Slithin.Modules.UI.Models;

namespace Slithin.Modules.UI;

internal class Module : AuroraModularis.Module
{
    public override Task OnStart(ServiceContainer container)
    {
        return Task.CompletedTask;
    }

    public override void RegisterServices(ServiceContainer container)
    {
        container.Register<INotificationService>(new NotificationServiceImpl()).AsSingleton();
        container.Register<IDialogService>(new DialogServiceImpl()).AsSingleton();
    }
}
