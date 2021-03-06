using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Ionic.Zip;
using Slithin.Core;
using Slithin.Core.Remarkable;
using Slithin.Core.Services;
using Slithin.Core.Tools;
using Slithin.Models;
using Slithin.UI.Tools;
using Slithin.ViewModels.Modals.Tools;

namespace Slithin.Tools;

public class RestoreTool : ITool
{
    private readonly IMailboxService _mailboxService;
    private readonly IPathManager _pathManager;
    private readonly Xochitl _xochitl;

    public RestoreTool(
        IPathManager pathManager,
        IMailboxService mailboxService,
        Xochitl xochitl)
    {
        _pathManager = pathManager;
        _mailboxService = mailboxService;
        _xochitl = xochitl;
    }

    public IImage Image
    {
        get
        {
            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

            return new Bitmap(assets.Open(new Uri("avares://Slithin/Resources/restore.png")));
        }
    }

    public ScriptInfo Info => new("restore", "Restore", "Internal", "Restore a Backup", false, true, false);

    public bool IsConfigurable => false;

    public Control GetModal()
    {
        return null;
    }

    public async void Invoke(object data)
    {
        var _ssh = ServiceLocator.Container.Resolve<ISSHService>();

        var vm = new SelectBackupViewModel
        {
            Backups = new ObservableCollection<Backup>(Directory.GetFiles(_pathManager.BackupsDir, "*.zip")
                .Except(Directory.GetFiles(_pathManager.BackupsDir, "*.notebook.zip")).Select(_ => new Backup(Path.GetFileNameWithoutExtension(_).Replace("Backup_from_", ""), _)))
        };

        var result = await DialogService.ShowDialog("Select Backup", new SelectBackupModal { DataContext = vm });

        if (result)
        {
            var really = await DialogService.ShowDialog(
                $"Do you really want to restore backup {vm.SelectedBackup.Name}? All files will be replaced!");

            if (really)
            {
                _mailboxService.PostAction(async () =>
                {
                    NotificationService.Show("Restoring Backup");

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

                    NotificationService.Show("Removing Notebooks From Device");
                    _ssh.RunCommand("rm -fr " + PathList.Documents).Dispose();
                    _ssh.RunCommand("mkdir " + PathList.Documents).Dispose();

                    NotificationService.Show("Removing Screens From Device");
                    _ssh.RunCommand("rm -fr " + PathList.Screens).Dispose();
                    _ssh.RunCommand("mkdir " + PathList.Screens).Dispose();

                    NotificationService.Show("Removing Templates From Device");
                    _ssh.RunCommand("rm -fr " + PathList.Templates).Dispose();
                    _ssh.RunCommand("mkdir " + PathList.Templates).Dispose();

                    NotificationService.Show("Uploading Notebooks");
                    _ssh.Upload(new DirectoryInfo(_pathManager.NotebooksDir), PathList.Documents);

                    NotificationService.Show("Uploading Screens");
                    _ssh.Upload(new DirectoryInfo(_pathManager.CustomScreensDir), PathList.Screens);

                    NotificationService.Show("Uploading Templates");
                    _ssh.Upload(new DirectoryInfo(_pathManager.TemplatesDir), PathList.Templates);

                    NotificationService.Show("Finished");
                    await Task.Delay(1000);

                    _xochitl.ReloadDevice();
                });
            }
        }
    }
}
