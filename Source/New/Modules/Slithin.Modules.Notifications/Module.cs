using AuroraModularis.Core;
using Slithin.Modules.Notifications;
using Slithin.Modules.Notifications.Models;

namespace Slithin.Modules.Menu;

internal class Module : AuroraModularis.Module
{
    public override Task OnStart(Container container)
    {
        return Task.CompletedTask;
    }

    public override void RegisterServices(Container container)
    {
        container.Register<INotificationService>(new NotificationServiceImpl());
    }
}
