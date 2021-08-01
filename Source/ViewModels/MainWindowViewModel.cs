using Slithin.Core;

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
        }
    }
}
