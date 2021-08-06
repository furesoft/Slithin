using Slithin.Core;
using Slithin.Core.Scripting;
using Slithin.Messages;

namespace Slithin.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public MainWindowViewModel(EventStorage events)
        {
            if (ServiceLocator.Local.GetVersion() < ServiceLocator.Device.GetVersion())
            {
                events.Invoke("newVersionAvailable");
                ServiceLocator.Local.UpdateVersion(ServiceLocator.Device.GetVersion());
            }

            ServiceLocator.Mailbox.Post(new CheckForUpdateMessage());
        }
    }
}
