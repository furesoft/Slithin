using System.Runtime.InteropServices;
using Slithin.Messages;

namespace Slithin.Core.MessageHandlers;

public class CheckForUpdatesMessageHandler : IMessageHandler<CheckForUpdateMessage>
{
    public void HandleMessage(CheckForUpdateMessage message)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var helpers = new DesktopBridge.Helpers();
            if (helpers.IsRunningAsUwp())
            {
                return;
            }
        }

        NotificationService.Show("Checking for Updates");

        Updater.StartUpdate();

        NotificationService.Hide();
    }
}
