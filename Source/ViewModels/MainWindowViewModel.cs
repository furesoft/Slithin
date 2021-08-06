using Slithin.Core;
using Slithin.Core.Scripting;
using Slithin.Core.Services;
using Slithin.Messages;

namespace Slithin.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public MainWindowViewModel(EventStorage events, IMailboxService mailboxService)
        {
            if (ServiceLocator.Local.GetVersion() < ServiceLocator.Device.GetVersion())
            {
                events.Invoke("newVersionAvailable");
                ServiceLocator.Local.UpdateVersion(ServiceLocator.Device.GetVersion());
            }

            mailboxService.Post(new CheckForUpdateMessage());
        }
    }
}
