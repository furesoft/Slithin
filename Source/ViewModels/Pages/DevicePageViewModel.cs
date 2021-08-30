using System;
using System.IO;
using PdfSharpCore.Pdf;
using Renci.SshNet;
using Slithin.Controls;
using Slithin.Core;
using Slithin.Core.Remarkable;
using Slithin.Core.Remarkable.Rendering;
using Slithin.Core.Scripting;
using Slithin.Core.Services;
using Slithin.Core.Sync.Repositorys;
using Slithin.Models;

namespace Slithin.ViewModels.Pages
{
    public class DevicePageViewModel : BaseViewModel
    {
        private readonly SshClient _client;
        private readonly EventStorage _events;
        private readonly IExportProviderFactory _exportProviderFactory;
        private readonly ILoadingService _loadingService;
        private readonly LocalRepository _localRepostory;
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
                                   IExportProviderFactory exportProviderFactory)
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

        public void export()
        {
            var id = "f27773a7-b054-4782-bbcf-a9acbf045977";
            var ep = _exportProviderFactory.GetExportProvider("PDF Document");

            var doc = new PdfDocument(File.OpenRead(@"C:\Users\chris\Documents\Slithin\Notebooks\" + id + ".pdf"));

            var opts = ExportOptions.Create(doc, "1-120");
            var md = Metadata.Load(id);
            var outputPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\test.pdf";

            ep.Export(opts, md, outputPath);
        }

        public override async void OnLoad()
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

            if (!string.IsNullOrEmpty(str))
            {
                IsBeta = bool.Parse(str);
            }

            Version = _versionService.GetDeviceVersion().ToString();

            if (_versionService.GetLocalVersion() < _versionService.GetDeviceVersion())
            {
                _events.Invoke("newVersionAvailable");
                _localRepostory.UpdateVersion(_versionService.GetDeviceVersion());

                if (!_settingsService.Get().AutomaticTemplateRecovery)
                {
                    var result = await DialogService.ShowDialog("A new version has been installed to your device. Would you upload your custom templates?");
                    if (result)
                    {
                        UploadTemplates();
                    }
                }
                else
                {
                    UploadTemplates();
                }
            }
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
    }
}
