using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Serilog;
using Slithin.Controls.Notifications;
using Slithin.Core.MVVM;
using Slithin.Core.Notifications;
using Slithin.ViewModels;

namespace Slithin.Core;

public static class NotificationService
{
    public static WindowNotificationManager Manager;
    private static StatusNotificationViewModel _progressViewModel;

    private static Avalonia.Controls.Notifications.NotificationCard card = null;

    public static void Show(string message)
    {
        var logger = ServiceLocator.Container.Resolve<ILogger>();
        logger.Information(message);

        Dispatcher.UIThread.InvokeAsync(() =>
        {
            Manager.Show(new TextBlock { Text = message, Foreground = Avalonia.Media.Brushes.Black }, TimeSpan.FromSeconds(2), null);
        });
    }

    public static void Show(Control control, BaseViewModel viewModel)
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            control.DataContext = viewModel;

            Manager.Show(control);
        });
    }

    public static Task<bool> ShowAction(string message, string okButtonText = "OK", string cancelButtonText = "Cancel")
    {
        var tcs = new TaskCompletionSource<bool>();

        Dispatcher.UIThread.InvokeAsync(() =>
        {
            var vm = new ActionNotificationViewModel();
            vm.Message = message;
            vm.CancelButtonText = cancelButtonText;
            vm.OKButtonText = okButtonText;

            vm.CancelCommand = new DelegateCommand(_ =>
            {
                tcs.SetResult(false);
            });
            vm.OKCommand = new DelegateCommand(_ =>
            {
                tcs.SetResult(true);
            });

            Show(new ActionNotificationControl(), vm);
        });

        return tcs.Task;
    }

    public static void ShowError(string message)
    {
        var logger = ServiceLocator.Container.Resolve<ILogger>();
        logger.Information(message);

        Dispatcher.UIThread.InvokeAsync(() =>
        {
            Manager.Show(new TextBlock { Text = message, Foreground = Avalonia.Media.Brushes.Black }, TimeSpan.FromSeconds(2), "Error");
        });
    }

    public static void ShowProgress(string message, int value, int maxValue)
    {
        var logger = ServiceLocator.Container.Resolve<ILogger>();

        Dispatcher.UIThread.InvokeAsync(async () =>
        {
            if (_progressViewModel == null)
            {
                var control = new StatusNotificationControl();

                _progressViewModel = new();
                control.DataContext = _progressViewModel;

                _progressViewModel.Message = message;
                _progressViewModel.Value = value;
                _progressViewModel.MaxValue = maxValue;

                card = await Manager.Show(control, TimeSpan.Zero, "");
            }

            _progressViewModel.Message = message;
            _progressViewModel.Value = value;

            if (_progressViewModel.Value == _progressViewModel.MaxValue)
            {
                logger.Information(message);

                card?.Close();

                _progressViewModel = null;
            }
        });
    }
}
