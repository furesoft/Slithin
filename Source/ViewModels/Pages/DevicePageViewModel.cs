using Renci.SshNet;
using Slithin.Core;
using Slithin.Core.Scripting;
using Slithin.Core.Services;
using Slithin.Core.Sync.Repositorys;
using Slithin.Messages;

namespace Slithin.ViewModels.Pages
{
    public class DevicePageViewModel : BaseViewModel
    {
        private readonly SshClient _client;
        private readonly EventStorage _events;
        private readonly ILoadingService _loadingService;
        private readonly LocalRepository _localRepostory;
        private readonly IMailboxService _mailboxService;
        private readonly IVersionService _versionService;
        private bool _isBeta;

        private string _version;

        public DevicePageViewModel(IVersionService versionService,
                                   ILoadingService loadingService,
                                   EventStorage events,
                                   IMailboxService mailboxService,
                                   LocalRepository localRepostory,
                                   SshClient client)
        {
            _versionService = versionService;
            _loadingService = loadingService;
            _events = events;
            _mailboxService = mailboxService;
            _localRepostory = localRepostory;
            _client = client;
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

            var str = _client.RunCommand("grep '^BetaProgram' /home/root/.config/remarkable/xochitl.conf").Result;
            str = str.Replace("BetaProgram=", "").Replace("\n", "");

            IsBeta = bool.Parse(str);

            Version = _versionService.GetDeviceVersion().ToString();

            if (_versionService.GetLocalVersion() < _versionService.GetDeviceVersion())
            {
                _events.Invoke("newVersionAvailable");
                _localRepostory.UpdateVersion(_versionService.GetDeviceVersion());
            }
        }
    }
}
