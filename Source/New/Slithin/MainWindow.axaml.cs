﻿using AuroraModularis.Core;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Markup.Xaml;
using Slithin.Modules.UI.Models;

namespace Slithin;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var notificationManager = new WindowNotificationManager(this);
        notificationManager.Position = NotificationPosition.BottomLeft;
        notificationManager.MaxItems = 1;

        ServiceContainer.Current.Resolve<INotificationService>().Init(notificationManager);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

#if DEBUG
        this.AttachDevTools();
#endif
    }
}
