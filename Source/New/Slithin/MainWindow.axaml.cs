using System.ComponentModel;
using AuroraModularis.Core;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Markup.Xaml;
using Slithin.Core.MVVM;
using Slithin.Modules.BaseServices.Models;
using Slithin.Modules.UI.Models;
using Slithin.ViewModels;

namespace Slithin;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        BaseViewModel.ApplyViewModel<MainWindowViewModel>(this);
        
        this.Opened += OnOpened;
        
        var notificationManager = new WindowNotificationManager(this);
        notificationManager.Position = NotificationPosition.BottomLeft;
        notificationManager.MaxItems = 1;

        ServiceContainer.Current.Resolve<INotificationService>().Init(notificationManager);
        
        ServiceContainer.Current.Resolve<IKeyBinding>().Init(this);
    }

    private void OnOpened(object? sender, EventArgs e)
    {
        var eventService = ServiceContainer.Current.Resolve<IEventService>();
        
        eventService.Invoke<object>("ApplicationLoaded", null);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

#if DEBUG
        this.AttachDevTools();
#endif
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        Environment.Exit(0);
    }
}
