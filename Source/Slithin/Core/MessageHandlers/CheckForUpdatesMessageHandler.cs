using System.Runtime.InteropServices;
using Slithin.Core.Services;
using Slithin.Messages;

namespace Slithin.Core.MessageHandlers;

public class CheckForUpdatesMessageHandler : IMessageHandler<CheckForUpdateMessage>
{
    private readonly ILocalisationService _localisationService;

    public CheckForUpdatesMessageHandler(ILocalisationService localisationService)
    {
        _localisationService = localisationService;
    }

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

        NotificationService.Show(_localisationService.GetString("Checking for Updates"));

        Updater.StartUpdate();

        NotificationService.Hide();
    }
}
