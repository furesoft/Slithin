using System.Collections.ObjectModel;
using System.Windows.Input;
using AuroraModularis.Logging.Models;
using Slithin.Core.MVVM;
using Slithin.Modules.Device.Models;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.Repository.Models;
using Slithin.Modules.Settings.Models;

namespace Slithin.Modules.Device.UI;

public class DevicePageViewModel : BaseViewModel
{
    private readonly ILocalisationService _localisationService;
    private readonly ILogger _logger;
    private readonly ILoginService _loginService;
    private readonly IPathManager _pathManager;
    private readonly ISettingsService _settingsService;
    private readonly IVersionService _versionService;
    private readonly IXochitlService _xochitlService;
    private readonly IRemarkableDevice _remarkableDevice;
    private bool _hasEmailAddresses;
    private ObservableCollection<string> _shareEmailAddresses;
    private string _version;

    public ObservableCollection<CustomScreen> CustomScreens { get; set; } = new();

    public DevicePageViewModel(IVersionService versionService,
        IXochitlService xochitlService,
        IRemarkableDevice remarkableDevice,
        ILocalisationService localisationService,
        IPathManager pathManager,
        ISettingsService settingsService,
        ILoginService loginService,
        ILogger logger)
    {
        _versionService = versionService;
        _xochitlService = xochitlService;
        _remarkableDevice = remarkableDevice;
        _localisationService = localisationService;
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

        _pathManager.InitDeviceDirectory();

        /*
        if (!Directory.GetFiles(_pathManager.TemplatesDir).Any() || !Directory.GetFiles(_pathManager.NotebooksDir).Any())
        {
            _mailboxService.PostAction(() =>
            {
                NotificationService.Show("Downloading Custom Screens");
                _device.DownloadCustomScreens();

                _device.GetTemplates();

                InitNotebooks();

                ServiceLocator.Container.Resolve<BackupTool>().Invoke(null);
            });
        }*/

        InitScreens();

        //temporarly load screens- replace later with loading module
        foreach (var cs in CustomScreens)
        {
            cs.Load();
        }

        /*
        _mailboxService.PostAction(() =>
        {
            //_loadingService.LoadApiToken();

            _loadingService.LoadScreens();
            _loadingService.LoadTools();
            _loadingService.LoadTemplates();

            SyncService.TemplateFilter.SelectedCategory = SyncService.TemplateFilter.Categories.FirstOrDefault();

            _loadingService.LoadNotebooks();
        });
        */

        ShareEmailAddresses = new(_xochitlService.GetShareEmailAddresses());

#if DEBUG
        ShareEmailAddresses = new(new[] { "demo@demo.de", "max.mustermann@muster.de" });
#endif

        HasEmailAddresses = ShareEmailAddresses.Any();

        ShareEmailAddresses.CollectionChanged += (s, e) =>
        {
            HasEmailAddresses = ShareEmailAddresses.Any();
        };

        Version = _versionService.GetDeviceVersion().ToString();

        if (_xochitlService.GetIsBeta())
        {
            Version += " Beta";
        }

        // await DoAfterDeviceUpdate();
    }

    /*
    public void InitNotebooks()
    {
        var _ssh = ServiceLocator.Container.Resolve<ISSHService>();

        var notebooksDir = _pathManager.NotebooksDir;
        NotificationService.Show(_localisationService.GetString("Downloading Notebooks"));

        var cmd = _ssh.RunCommand("ls -p " + PathList.Documents);
        var allNodes
            = cmd.Result
                .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Where(_ => !_.EndsWith(".zip") && !_.EndsWith(".zip.part")).ToArray();

        for (var i = 0; i < allNodes.Length; i++)
        {
            var node = allNodes[i];
            if (!node.EndsWith("/"))
            {
                _ssh.Download(PathList.Documents + node, new FileInfo(Path.Combine(notebooksDir, node)));
                continue;
            }

            Directory.CreateDirectory(Path.Combine(notebooksDir, node.Remove(node.Length - 1, 1)));
            NotificationService.ShowProgress("Downloading Notebooks", i, allNodes.Length - 1);
            _ssh.Download(PathList.Documents + node, new DirectoryInfo(Path.Combine(notebooksDir, node.Remove(node.Length - 1, 1))));
        }
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
    */

    private void InitScreens()
    {
        CustomScreens.Add(new CustomScreen { Title = _localisationService.GetString("Starting"), Filename = "starting.png" });
        CustomScreens.Add(new CustomScreen { Title = _localisationService.GetString("Power Off"), Filename = "poweroff.png" });
        CustomScreens.Add(new CustomScreen { Title = _localisationService.GetString("Suspended"), Filename = "suspended.png" });
        CustomScreens.Add(new CustomScreen { Title = _localisationService.GetString("Rebooting"), Filename = "rebooting.png" });
        CustomScreens.Add(new CustomScreen { Title = _localisationService.GetString("Battery Empty"), Filename = "batteryempty.png" });

        _logger.Info("Initialize Screens");
    }

    private void ReloadDevice(object obj)
    {
        _remarkableDevice.Reload();
    }

    private void RemoveEmail(object obj)
    {
        ShareEmailAddresses.Remove(obj.ToString());

#if RELEASE
        _xochitl.SetShareMailAddresses(ShareEmailAddresses);
        _mailboxService.PostAction(() => _xochitl.Save());
#endif
    }

    /*
    private void UploadScreens()
    {
        var _ssh = ServiceLocator.Container.Resolve<ISSHService>();

        _mailboxService.PostAction(() =>
        {
            NotificationService.Show(_localisationService.GetString("Uploading Screens"));

            _ssh.Upload(new DirectoryInfo(_pathManager.CustomScreensDir), PathList.Screens);

            _xochitl.ReloadDevice();
        });
    }

    private void UploadTemplates()
    {
        var _ssh = ServiceLocator.Container.Resolve<ISSHService>();

        _mailboxService.PostAction(() =>
        {
            NotificationService.Show(_localisationService.GetString("Uploading Templates"));

            _ssh.Upload(new DirectoryInfo(_pathManager.TemplatesDir), PathList.Templates);

            _xochitl.ReloadDevice();
        });
    }
    */
}
