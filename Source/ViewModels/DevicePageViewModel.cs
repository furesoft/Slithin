using System.Collections.ObjectModel;
using Slithin.Core;

namespace Slithin.ViewModels
{
    public class DevicePageViewModel : BaseViewModel
    {
        public DevicePageViewModel()
        {
            CustomScreens.Add(new CustomScreen { Title = "Sleep" });
            CustomScreens.Add(new CustomScreen { Title = "Power" });
            CustomScreens.Add(new CustomScreen { Title = "Suspend" });
            CustomScreens.Add(new CustomScreen { Title = "Rebooting" });
        }

        public ObservableCollection<CustomScreen> CustomScreens { get; set; } = new();
    }
}
