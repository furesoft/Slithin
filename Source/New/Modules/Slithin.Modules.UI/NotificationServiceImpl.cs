using Avalonia.Controls.Notifications;
using Slithin.Modules.UI.Models;

namespace Slithin.Modules.Notifications;

internal class NotificationServiceImpl : INotificationService
{
    private WindowNotificationManager _notificationManager;

    public void Init(WindowNotificationManager manager)
    {
        _notificationManager = manager;
    }

    public void Show(string message)
    {
        _notificationManager.Show(new Notification("", message));
    }

    public IStatusController ShowStatus(string message, bool showInNewWindow = false)
    {
        var controller = new StatusController(showInNewWindow);
        controller.Step(message);

        return controller;
    }
}
