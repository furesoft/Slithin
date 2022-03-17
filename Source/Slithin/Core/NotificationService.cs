using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Serilog;
using Slithin.Controls.Notifications;
using Slithin.ViewModels;

namespace Slithin.Core;

public static class NotificationService
{
    private static Border notificationContainer;

    public static bool GetIsNotificationOutput(Border target)
    {
        return Equals(target, notificationContainer);
    }

    public static void Hide()
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            notificationContainer.IsVisible = false;
        });
    }

    public static void SetIsNotificationOutput(Border target, bool value)
    {
        notificationContainer = target;
        notificationContainer.DataContext = new StatusNotificationViewModel();
    }

    public static void Show(string message)
    {
        var logger = ServiceLocator.Container.Resolve<ILogger>();
        logger.Information(message);

        Dispatcher.UIThread.InvokeAsync(() =>
        {
            var vm = ((StatusNotificationViewModel)notificationContainer.DataContext);
            vm.Message = message;
            vm.Value = 0;
            vm.MaxValue = 0;
            vm.IsInfo = true;

            var control = new StatusNotificationControl();

            notificationContainer.Child = control;
            control.DataContext = vm;
            notificationContainer.IsVisible = true;
        });
    }

    public static void Show(Control control, BaseViewModel viewModel)
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            notificationContainer.Child = control;
            control.DataContext = viewModel;

            notificationContainer.IsVisible = true;
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
                Hide();
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

    public static void ShowProgress(string message, int value, int maxValue)
    {
        var logger = ServiceLocator.Container.Resolve<ILogger>();
        logger.Information(message);

        Dispatcher.UIThread.InvokeAsync(() =>
        {
            var vm = ((StatusNotificationViewModel)notificationContainer.DataContext);
            vm.Message = message;
            vm.Value = value;
            vm.MaxValue = maxValue;
            vm.IsInfo = false;

            var control = new StatusNotificationControl();

            notificationContainer.Child = control;
            control.DataContext = vm;
            notificationContainer.IsVisible = true;
        });
    }
}
