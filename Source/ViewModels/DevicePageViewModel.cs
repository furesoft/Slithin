using Slithin.Core;

namespace Slithin.ViewModels
{
    public class DevicePageViewModel : BaseViewModel
    {
        public DevicePageViewModel()
        {
            SyncService.CustomScreens.Add(new CustomScreen { Title = "Starting", Filename = "starting.png" });
            SyncService.CustomScreens.Add(new CustomScreen { Title = "Power Off", Filename = "poweroff.png" });
            SyncService.CustomScreens.Add(new CustomScreen { Title = "Suspended", Filename = "suspended.png" });
            SyncService.CustomScreens.Add(new CustomScreen { Title = "Rebooting", Filename = "rebooting.png" });
            SyncService.CustomScreens.Add(new CustomScreen { Title = "Splash", Filename = "splash.png" });

            foreach (var cs in ServiceLocator.SyncService.CustomScreens)
            {
                cs.Load();
            }

            Version = ServiceLocator.Device.GetVersion().ToString();

            var str = ServiceLocator.Client.RunCommand("grep '^BetaProgram' /home/root/.config/remarkable/xochitl.conf").Result;
            str = str.Replace("BetaProgram=", "").Replace("\n", "");

            IsBeta = bool.Parse(str);
        }

        public bool IsBeta { get; set; }
        public string Version { get; set; }
    }
}
