using Slithin.Messages;

namespace Slithin.Core.MessageHandlers
{
    public class CheckForUpdatesMessageHandler : IMessageHandler<InitStorageMessage>
    {
        public async void HandleMessage(InitStorageMessage message)
        {
            NotificationService.Show("Checking for Updates");

            await Updater.StartUpdate();

            NotificationService.Hide();
        }
    }
}
