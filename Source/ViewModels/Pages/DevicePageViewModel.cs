using Slithin.Core;
using Slithin.Core.Services;
using Slithin.ViewModels;

namespace Slithin.ViewModels.Pages
{
    public class DevicePageViewModel : BaseViewModel
    {
        private readonly ILoadingService _loadingService;
        private readonly IVersionService _versionService;
        private bool _isBeta;

        private string _version;

        public DevicePageViewModel(IVersionService versionService, ILoadingService loadingService)
        {
            _versionService = versionService;
            _loadingService = loadingService;
        }

        public bool IsBeta
        {
            get { return _isBeta; }
            set { SetValue(ref _isBeta, value); }
        }

        public string Version
        {
            get { return _version; }
            set { SetValue(ref _version, value); }
        }

        public override void OnLoad()
        {
            base.OnLoad();

            SyncService.CustomScreens.Add(new CustomScreen { Title = "Starting", Filename = "starting.png" });
            SyncService.CustomScreens.Add(new CustomScreen { Title = "Power Off", Filename = "poweroff.png" });
            SyncService.CustomScreens.Add(new CustomScreen { Title = "Suspended", Filename = "suspended.png" });
            SyncService.CustomScreens.Add(new CustomScreen { Title = "Rebooting", Filename = "rebooting.png" });
            SyncService.CustomScreens.Add(new CustomScreen { Title = "Splash", Filename = "splash.png" });

            _loadingService.LoadScreens();

            var str = ServiceLocator.Client.RunCommand("grep '^BetaProgram' /home/root/.config/remarkable/xochitl.conf").Result;
            str = str.Replace("BetaProgram=", "").Replace("\n", "");

            IsBeta = bool.Parse(str);

            Version = _versionService.GetDeviceVersion().ToString();
        }
    }
}
