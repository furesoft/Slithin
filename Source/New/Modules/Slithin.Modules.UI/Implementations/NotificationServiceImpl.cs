using AuroraModularis.Core;
using AuroraModularis.Logging.Models;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Slithin.Modules.UI.Models;

namespace Slithin.Modules.UI.Implementations;

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

    public void ShowError(string message)
    {
        Container.Current.Resolve<ILogger>().Error(message);
        _notificationManager.Show((new Notification("Error", message, NotificationType.Error)));
    }

    //ToDo: error notification in new window need to be tested
    public void ShowErrorNewWindow(string message)
    {
        Container.Current.Resolve<ILogger>().Error(message);
        
        var window = new Window();
        var notificationCard = new NotificationCard();
        notificationCard.Content = new Notification("Error", message, NotificationType.Error);

        window.Content = notificationCard;
        window.Show();
    }

    public IStatusController ShowStatus(string message, bool showInNewWindow = false)
    {
        var controller = new StatusController(showInNewWindow);
        controller.Step(message);

        return controller;
    }
}
