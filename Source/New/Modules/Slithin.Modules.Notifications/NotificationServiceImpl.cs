using Avalonia.Controls.Notifications;
using Slithin.Modules.Notifications.Models;

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
}
