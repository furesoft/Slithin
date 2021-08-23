using Slithin.Messages;

namespace Slithin.Core.MessageHandlers
{
    public class CheckForUpdatesMessageHandler : IMessageHandler<CheckForUpdateMessage>
    {
        public void HandleMessage(CheckForUpdateMessage message)
        {
            NotificationService.Show("Checking for Updates");

            Updater.StartUpdate();

            NotificationService.Hide();
        }
    }
}
