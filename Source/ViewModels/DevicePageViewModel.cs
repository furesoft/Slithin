using Slithin.Core;
using Slithin.Core.Services;

namespace Slithin.ViewModels
{
    public class DevicePageViewModel : BaseViewModel
    {
        private readonly IVersionService _versionService;

        public DevicePageViewModel(IVersionService versionService)
        {
            _versionService = versionService;
        }

        public bool IsBeta { get; set; }

        public string Version { get; set; }

        public override void OnLoad()
        {
            base.OnLoad();

            SyncService.CustomScreens.Add(new CustomScreen { Title = "Starting", Filename = "starting.png" });
            SyncService.CustomScreens.Add(new CustomScreen { Title = "Power Off", Filename = "poweroff.png" });
            SyncService.CustomScreens.Add(new CustomScreen { Title = "Suspended", Filename = "suspended.png" });
            SyncService.CustomScreens.Add(new CustomScreen { Title = "Rebooting", Filename = "rebooting.png" });
            SyncService.CustomScreens.Add(new CustomScreen { Title = "Splash", Filename = "splash.png" });

            foreach (var cs in ServiceLocator.SyncService.CustomScreens)
            {
                cs.Load();
            }

            var str = ServiceLocator.Client.RunCommand("grep '^BetaProgram' /home/root/.config/remarkable/xochitl.conf").Result;
            str = str.Replace("BetaProgram=", "").Replace("\n", "");

            IsBeta = bool.Parse(str);

            Version = _versionService.GetDeviceVersion().ToString();
        }
    }
}
