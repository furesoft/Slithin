using Slithin.Messages;

namespace Slithin.Core.MessageHandlers;

public class CheckForUpdatesMessageHandler : IMessageHandler<CheckForUpdateMessage>
{
    public void HandleMessage(CheckForUpdateMessage message)
    {
        var helpers = new DesktopBridge.Helpers();
        if (!helpers.IsRunningAsUwp())
        {
            NotificationService.Show("Checking for Updates");

            Updater.StartUpdate();

            NotificationService.Hide();
        }
    }
}