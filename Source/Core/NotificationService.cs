using System.Windows.Input;
using Avalonia.Controls;

namespace Slithin.Core
{
    public static class NotificationService
    {
        private static TextBlock outputTextBlock;

        public static ICommand BuildCommand(string message, ICommand handlerCommand)
        {
            return new DelegateCommand((_) =>
            {
                DoAndShow(message, handlerCommand);
            });
        }

        public static void DoAndShow(string message, ICommand cmd)
        {
            Show(message);

            cmd.Execute(null);

            Hide();
        }

        public static bool GetIsNotificationOutput(TextBlock target)
        {
            return target == outputTextBlock;
        }

        public static void Hide()
        {
            outputTextBlock.IsVisible = false;
        }

        public static void SetIsNotificationOutput(TextBlock target, bool value)
        {
            outputTextBlock = target;
        }

        public static void Show(string message)
        {
            outputTextBlock.Text = message;
            outputTextBlock.IsVisible = true;
        }
    }
}
