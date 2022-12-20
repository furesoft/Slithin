using AuroraModularis.Core;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Markup.Xaml;
using Slithin.Modules.Notifications.Models;

namespace Slithin;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var notificationManager = new WindowNotificationManager(this);
        notificationManager.Position = NotificationPosition.BottomLeft;
        notificationManager.MaxItems = 1;

        Container.Current.Resolve<INotificationService>().Init(notificationManager);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

#if DEBUG
        this.AttachDevTools();
#endif
    }
}
