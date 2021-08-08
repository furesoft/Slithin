using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Ionic.Zip;
using Slithin.Controls;
using Slithin.Core;
using Slithin.Core.Services;
using Slithin.UI.Tools;
using Slithin.ViewModels;

namespace Slithin.Tools
{
    public class Backup
    {
        public Backup(string name, string filename)
        {
            Name = name;
            Filename = filename;
        }

        public string Filename { get; set; }
        public string Name { get; set; }
    }

    public class RestoreTool : ITool
    {
        private readonly IMailboxService _mailboxService;
        private readonly IPathManager _pathManager;

        public RestoreTool(IPathManager pathManager, IMailboxService mailboxService)
        {
            _pathManager = pathManager;
            _mailboxService = mailboxService;
        }

        public IImage Image
        {
            get
            {
                var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

                return new Bitmap(assets.Open(new Uri($"avares://Slithin/Resources/restore.png")));
            }
        }

        public ScriptInfo Info => new("Restore", "Internal", "Restore a Backup");

        public bool IsConfigurable => false;

        public Control GetModal()
        {
            return null;
        }

        public async void Invoke(object data)
        {
            var vm = new SelectBackupViewModel
            {
                Backups = new(Directory.GetFiles(_pathManager.BackupsDir, "*.zip")
                .Select(_ => new Backup(Path.GetFileNameWithoutExtension(_).Replace("Backup_from_", ""), _)))
            };

            var result = await DialogService.ShowDialog("Select Backup", new SelectBackupModal { DataContext = vm });

            if (result)
            {
                var really = await DialogService.ShowDialog($"Do you really want to restore backup {vm.SelectedBackup.Name}? All files will be replaced!");

                if (really)
                {
                    _mailboxService.PostAction(async () =>
                    {
                        NotificationService.Show("Restoring Backup");

                        //Todo: implement restore backup

                        //delete local files but not backups folder
                        NotificationService.Show("Deleting Local Files - Screens");
                        Directory.Delete(_pathManager.CustomScreensDir, true);

                        NotificationService.Show("Deleting Local Files - Notebooks");
                        Directory.Delete(_pathManager.NotebooksDir, true);

                        NotificationService.Show("Deleting Local Files - Scripts");
                        Directory.Delete(_pathManager.ScriptsDir, true);

                        NotificationService.Show("Deleting Local Files - Templates");
                        Directory.Delete(_pathManager.TemplatesDir, true);

                        //extract zip
                        NotificationService.Show("Extracting Backup");
                        using (var zip = new ZipFile(vm.SelectedBackup.Filename))
                        {
                            zip.ExtractAll(_pathManager.ConfigBaseDir, ExtractExistingFileAction.OverwriteSilently);
                        }
                        //upload all data

                        NotificationService.Show("Finished");
                        await Task.Delay(1000);

                        NotificationService.Hide();
                    });
                }
            }
        }
    }
}
