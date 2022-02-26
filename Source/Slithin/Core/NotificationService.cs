using Avalonia.Controls;
using Avalonia.Threading;
using Serilog;
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
        notificationContainer.DataContext = new NotificationViewModel();
    }

    public static void Show(string message)
    {
        var logger = ServiceLocator.Container.Resolve<ILogger>();
        logger.Information(message);

        Dispatcher.UIThread.InvokeAsync(() =>
        {
            var vm = ((NotificationViewModel)notificationContainer.DataContext);
            vm.Message = message;
            vm.Value = 100;
            vm.MaxValue = 100;
            vm.IsInfo = true;

            notificationContainer.IsVisible = true;
        });
    }

    public static void ShowProgress(string message, int value, int maxValue)
    {
        var logger = ServiceLocator.Container.Resolve<ILogger>();
        logger.Information(message);

        Dispatcher.UIThread.InvokeAsync(() =>
        {
            var vm = ((NotificationViewModel)notificationContainer.DataContext);
            vm.Message = message;
            vm.Value = value;
            vm.MaxValue = maxValue;
            vm.IsInfo = false;

            notificationContainer.IsVisible = true;
        });
    }
}
