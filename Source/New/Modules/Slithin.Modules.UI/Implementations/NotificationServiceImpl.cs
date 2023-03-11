﻿using AuroraModularis.Core;
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
        ServiceContainer.Current.Resolve<ILogger>().Error(message);
        _notificationManager.Show((new Notification("Error", message, NotificationType.Error)));
    }
    
    public void ShowErrorNewWindow(string message)
    {
        ServiceContainer.Current.Resolve<ILogger>().Error(message);

        var window = new Window();

        window.Content = message;
        window.Show();
    }

    public IStatusController ShowStatus(string message, bool isCancellable = false, bool showInNewWindow = false)
    {
        ServiceContainer.Current.Resolve<ILogger>().Info(message);

        var controller = new StatusController(showInNewWindow, isCancellable);
        controller.Step(message);

        return controller;
    }
}
