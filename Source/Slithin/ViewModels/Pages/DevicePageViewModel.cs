using System.IO;
using System.Threading.Tasks;
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
    private readonly SshClient _client;
    private readonly IExportProviderFactory _exportProviderFactory;
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
    private readonly Xochitl _xochitl;
    private bool _isBeta;

    private string _version;

    public DevicePageViewModel(IVersionService versionService,
        ILoadingService loadingService,
        IMailboxService mailboxService,
        ILocalisationService localisationService,
        LocalRepository localRepostory,
        SshClient client,
        ScpClient scp,
        IPathManager pathManager,
        ISettingsService settingsService,
        IExportProviderFactory exportProviderFactory,
        ILoginService loginService,
        Xochitl xochitl,
        ILogger logger)
    {
        _versionService = versionService;
        _loadingService = loadingService;
        _mailboxService = mailboxService;
        _localisationService = localisationService;
        _localRepostory = localRepostory;
        _client = client;
        _scp = scp;
        _pathManager = pathManager;
        _settingsService = settingsService;
        _exportProviderFactory = exportProviderFactory;
        _loginService = loginService;
        _xochitl = xochitl;
        _logger = logger;
    }

    public bool IsBeta
    {
        get => _isBeta;
        set => SetValue(ref _isBeta, value);
    }

    public string Version
    {
        get => _version;
        set => SetValue(ref _version, value);
    }

    public override async void OnLoad()
    {
        base.OnLoad();

        ServiceLocator.Container.Resolve<Xochitl>().Init();

        InitScreens();

        _loadingService.LoadScreens();

        _mailboxService.PostAction(() =>
        {
            _loadingService.LoadTools();
        });

        IsBeta = _xochitl.GetIsBeta();

        Version = _versionService.GetDeviceVersion().ToString();

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

    private void UploadScreens()
    {
        _mailboxService.PostAction(() =>
        {
            NotificationService.Show(_localisationService.GetString("Uploading Screens"));

            _scp.Upload(new DirectoryInfo(_pathManager.CustomScreensDir), PathList.Screens);

            TemplateStorage.Instance.Apply();
            NotificationService.Hide();
        });
    }

    private void UploadTemplates()
    {
        _mailboxService.PostAction(() =>
        {
            NotificationService.Show(_localisationService.GetString("Uploading Templates"));

            _scp.Upload(new DirectoryInfo(_pathManager.TemplatesDir), PathList.Templates);

            TemplateStorage.Instance.Apply();
            NotificationService.Hide();
        });
    }
}
