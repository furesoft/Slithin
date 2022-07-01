using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Markup.Xaml;
using Slithin.Core;
using Slithin.Core.Services;
using Slithin.ViewModels;

namespace Slithin.UI.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        NotificationService.Manager = new(this);
        NotificationService.Manager.Position = NotificationPosition.BottomRight;
        NotificationService.Manager.MaxItems = 1;

#if DEBUG
        this.AttachDevTools();
#endif

        this.Closed += MainWindow_Closed;
    }

    private void MainWindow_Closed(object sender, EventArgs e)
    {
        ServiceLocator.Container.Resolve<ISSHService>().Dispose();

        ServiceLocator.Container.Resolve<LiteDB.LiteDatabase>().Dispose();

        Environment.Exit(0);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        DataContext = ServiceLocator.Container.Resolve<MainWindowViewModel>();
    }
}
