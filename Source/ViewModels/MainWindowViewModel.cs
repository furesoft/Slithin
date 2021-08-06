using Slithin.Core;
using Slithin.Messages;

namespace Slithin.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public MainWindowViewModel()
        {
            if (ServiceLocator.Local.GetVersion() < ServiceLocator.Device.GetVersion())
            {
                ServiceLocator.Events.Invoke("newVersionAvailable");
                ServiceLocator.Local.UpdateVersion(ServiceLocator.Device.GetVersion());
            }

            ServiceLocator.Mailbox.Post(new CheckForUpdateMessage());
        }
    }
}
