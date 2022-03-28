using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Renci.SshNet;
using Serilog;
using Slithin.Controls;
using Slithin.Core;
using Slithin.Core.Remarkable;
using Slithin.Core.Services;
using Slithin.Core.Sync.Repositorys;
using Slithin.Models;

namespace Slithin.ViewModels.Pages;

public class DevicePageViewModel : BaseViewModel
{
    private readonly ILoadingService _loadingService;
    private readonly ILocalisationService _localisationService;
    private readonly LocalRepository _localRepostory;
    private readonly ILogger _logger;
    private readonly ILoginService _loginService;
    private readonly IMailboxService _mailboxService;
    private readonly IPathManager _pathManager;
    private readonly ScpClient _scp;
    private readonly ISettingsService _settingsService;
    private readonly IVersionService _versionService;

    private bool _hasEmailAddresses;
    private ObservableCollection<string> _shareEmailAddresses;
    private string _version;
    private Xochitl _xochitl;

    public DevicePageViewModel(IVersionService versionService,
        ILoadingService loadingService,
        IMailboxService mailboxService,
        ILocalisationService localisationService,
        LocalRepository localRepostory,
        ScpClient scp,
        IPathManager pathManager,
        ISettingsService settingsService,
        ILoginService loginService,
        ILogger logger)
    {
        _versionService = versionService;
        _loadingService = loadingService;
        _mailboxService = mailboxService;
        _localisationService = localisationService;
        _localRepostory = localRepostory;
        _scp = scp;
        _pathManager = pathManager;
        _settingsService = settingsService;
        _loginService = loginService;
        _logger = logger;

        RemoveEmailCommand = new DelegateCommand(RemoveEmail);
        ReloadDeviceCommand = new DelegateCommand(ReloadDevice);
    }

    public bool HasEmailAddresses
    {
        get { return _hasEmailAddresses; }
        set { SetValue(ref _hasEmailAddresses, value); }
    }

    public ICommand ReloadDeviceCommand { get; set; }

    public ICommand RemoveEmailCommand { get; set; }

    public ObservableCollection<string> ShareEmailAddresses
    {
        get { return _shareEmailAddresses; }
        set { SetValue(ref _shareEmailAddresses, value); }
    }

    public string Version
    {
        get => _version;
        set => SetValue(ref _version, value);
    }

    public override async void OnLoad()
    {
        base.OnLoad();

        _xochitl = ServiceLocator.Container.Resolve<Xochitl>();
        _xochitl.Init();

        InitScreens();

        _loadingService.LoadScreens();

        _mailboxService.PostAction(() =>
        {
            _loadingService.LoadTools();
        });

        ShareEmailAddresses = new(_xochitl.GetShareEmailAddresses());

#if DEBUG
        ShareEmailAddresses = new(new[] { "demo@demo.de", "max.mustermann@muster.de" });
#endif

        HasEmailAddresses = ShareEmailAddresses.Any();

        ShareEmailAddresses.CollectionChanged += (s, e) =>
        {
            HasEmailAddresses = ShareEmailAddresses.Any();
        };

        Version = _versionService.GetDeviceVersion().ToString();

        if (_xochitl.GetIsBeta())
        {
            Version += " Beta";
        }

        await DoAfterDeviceUpdate();
    }

    private async Task DoAfterDeviceUpdate()
    {
        if (_versionService.GetLocalVersion() >= _versionService.GetDeviceVersion())
        {
            return;
        }

        _localRepostory.UpdateVersion(_versionService.GetDeviceVersion());

        _loginService.UpdateIPAfterUpdate();

        await DoNewVersionUpload();
    }

    private async Task DoNewVersionUpload()
    {
        var needTemplateMessage = true;
        var needScreenMessage = true;

        if (_settingsService.GetSettings().AutomaticTemplateRecovery)
        {
            UploadTemplates();
            needTemplateMessage = false;
        }

        if (_settingsService.GetSettings().AutomaticScreenRecovery)
        {
            UploadScreens();
            needScreenMessage = false;
        }

        if (!needScreenMessage && !needTemplateMessage) return;

        var result =
            await DialogService.ShowDialog(
               _localisationService.GetString(
                   "A new version has been installed to your device. Would you upload your custom templates/screens?"));
        if (result)
        {
            UploadTemplates();
            UploadScreens();
        }
    }

    private void InitScreens()
    {
        SyncService.CustomScreens.Add(new CustomScreen { Title = _localisationService.GetString("Starting"), Filename = "starting.png" });
        SyncService.CustomScreens.Add(new CustomScreen { Title = _localisationService.GetString("Power Off"), Filename = "poweroff.png" });
        SyncService.CustomScreens.Add(new CustomScreen { Title = _localisationService.GetString("Suspended"), Filename = "suspended.png" });
        SyncService.CustomScreens.Add(new CustomScreen { Title = _localisationService.GetString("Rebooting"), Filename = "rebooting.png" });
        SyncService.CustomScreens.Add(new CustomScreen { Title = _localisationService.GetString("Splash"), Filename = "splash.png" });
        SyncService.CustomScreens.Add(new CustomScreen { Title = _localisationService.GetString("Battery Empty"), Filename = "batteryempty.png" });

        _logger.Information("Initialize Screens");
    }

    private void ReloadDevice(object obj)
    {
        _xochitl.ReloadDevice();
    }

    private void RemoveEmail(object obj)
    {
        ShareEmailAddresses.Remove(obj.ToString());

#if RELEASE
        _xochitl.SetShareMailAddresses(ShareEmailAddresses);
        _mailboxService.PostAction(() => _xochitl.Save());
#endif
    }

    private void UploadScreens()
    {
        _mailboxService.PostAction(() =>
        {
            NotificationService.Show(_localisationService.GetString("Uploading Screens"));

            _scp.Upload(new DirectoryInfo(_pathManager.CustomScreensDir), PathList.Screens);

            _xochitl.ReloadDevice();
        });
    }

    private void UploadTemplates()
    {
        _mailboxService.PostAction(() =>
        {
            NotificationService.Show(_localisationService.GetString("Uploading Templates"));

            _scp.Upload(new DirectoryInfo(_pathManager.TemplatesDir), PathList.Templates);

            _xochitl.ReloadDevice();
        });
    }
}
