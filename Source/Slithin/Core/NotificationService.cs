using Avalonia.Controls;
using Avalonia.Threading;
using Serilog.Core;

namespace Slithin.Core;

public static class NotificationService
{
    private static TextBlock outputTextBlock;

    public static bool GetIsNotificationOutput(TextBlock target)
    {
        return Equals(target, outputTextBlock);
    }

    public static void Hide()
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            outputTextBlock.IsVisible = false;
        });
    }

    public static void SetIsNotificationOutput(TextBlock target, bool value)
    {
        outputTextBlock = target;
    }

    public static void Show(string message)
    {
        var logger = ServiceLocator.Container.Resolve<Logger>();
        logger.Information(message);

        Dispatcher.UIThread.InvokeAsync(() =>
        {
            outputTextBlock.Text = message;
            outputTextBlock.IsVisible = true;
        });
    }
}
