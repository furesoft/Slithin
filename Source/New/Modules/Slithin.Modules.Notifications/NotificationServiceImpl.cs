using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Notifications;
using Slithin.Modules.Notifications.Models;

namespace Slithin.Modules.Notifications;

internal class NotificationServiceImpl : INotificationService
{
    private WindowNotificationManager _notificationManager;

    public void Show(string message)
    {
        if (_notificationManager == null)
        {
            var lifetime = Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            _notificationManager = new(lifetime.MainWindow);
        }

        _notificationManager.Show(message);
    }
}
