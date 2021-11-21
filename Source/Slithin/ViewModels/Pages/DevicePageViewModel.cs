using System.IO;
using System.Threading.Tasks;
using Renci.SshNet;
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
    private readonly EventStorage _events;
    private readonly IExportProviderFactory _exportProviderFactory;
    private readonly ILoadingService _loadingService;
    private readonly LocalRepository _localRepostory;
    private readonly ILoginService _loginService;
    private readonly IMailboxService _mailboxService;
    private readonly IPathManager _pathManager;
    private readonly ScpClient _scp;
    private readonly ISettingsService _settingsService;
    private readonly IVersionService _versionService;

    private bool _isBeta;

    private string _version;

    public DevicePageViewModel(IVersionService versionService,
        ILoadingService loadingService,
        EventStorage events,
        IMailboxService mailboxService,
        LocalRepository localRepostory,
        SshClient client,
        ScpClient scp,
        IPathManager pathManager,
        ISettingsService settingsService,
        IExportProviderFactory exportProviderFactory,
        ILoginService loginService)
    {
        _versionService = versionService;
        _loadingService = loadingService;
        _events = events;
        _mailboxService = mailboxService;
        _localRepostory = localRepostory;
        _client = client;
        _scp = scp;
        _pathManager = pathManager;
        _settingsService = settingsService;
        _exportProviderFactory = exportProviderFactory;
        _loginService = loginService;
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

        InitScreens();

        _loadingService.LoadScreens();

        _mailboxService.PostAction(() =>
        {
            NotificationService.Show("Loading Tools");

            _loadingService.LoadTools();
            NotificationService.Hide();
        });

        InitIsBeta();

        Version = _versionService.GetDeviceVersion().ToString();

        await DoAfterDeviceUpdate();
    }

    private async Task DoAfterDeviceUpdate()
    {
        if (_versionService.GetLocalVersion() >= _versionService.GetDeviceVersion())
        {
            return;
        }

        _events.Invoke("newVersionAvailable");
        _localRepostory.UpdateVersion(_versionService.GetDeviceVersion());

        _loginService.UpdateIPAfterUpdate();

        await DoNewVersionUpload();
    }

    private async Task DoNewVersionUpload()
    {
        if (_settingsService.GetSettings().AutomaticTemplateRecovery)
        {
            UploadTemplates();
            UploadScreens();

            return;
        }

        var result =
            await DialogService.ShowDialog(
                "A new version has been installed to your device. Would you upload your custom templates?");
        if (result)
        {
            UploadTemplates();
            UploadScreens();
        }
    }

    private void InitIsBeta()
    {
        var sshCommand = _client.RunCommand("grep '^BetaProgram' /home/root/.config/remarkable/xochitl.conf");
        var str = sshCommand.Result;
        str = str.Replace("BetaProgram=", "").Replace("\n", "");

        if (!string.IsNullOrEmpty(str))
        {
            IsBeta = bool.Parse(str);
        }
    }

    private void InitScreens()
    {
        SyncService.CustomScreens.Add(new CustomScreen {Title = "Starting", Filename = "starting.png"});
        SyncService.CustomScreens.Add(new CustomScreen {Title = "Power Off", Filename = "poweroff.png"});
        SyncService.CustomScreens.Add(new CustomScreen {Title = "Suspended", Filename = "suspended.png"});
        SyncService.CustomScreens.Add(new CustomScreen {Title = "Rebooting", Filename = "rebooting.png"});
        SyncService.CustomScreens.Add(new CustomScreen {Title = "Splash", Filename = "splash.png"});
        SyncService.CustomScreens.Add(new CustomScreen {Title = "Battery Empty", Filename = "batteryempty.png"});
    }

    private void UploadTemplates()
    {
        _mailboxService.PostAction(() =>
        {
            //upload template folder
            NotificationService.Show("Uploading Templates");

            _scp.Upload(new DirectoryInfo(_pathManager.TemplatesDir), PathList.Templates);

            TemplateStorage.Instance.Apply();
            NotificationService.Hide();
        });
    }

    private void UploadScreens()
    {
        _mailboxService.PostAction(() =>
        {
            //upload screens folder
            NotificationService.Show("Uploading Screens");

            _scp.Upload(new DirectoryInfo(_pathManager.CustomScreensDir), PathList.Screens);

            TemplateStorage.Instance.Apply();
            NotificationService.Hide();
        });
    }
}
