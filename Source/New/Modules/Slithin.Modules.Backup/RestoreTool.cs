using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Ionic.Zip;
using Slithin.Modules.Device.Models;
using Slithin.Modules.Repository.Models;
using Slithin.Modules.Tools.Models;
using Slithin.Modules.UI.Models;

namespace Slithin.Modules.Backup;

internal class RestoreTool : ITool
{
    private readonly IPathManager _pathManager;
    private readonly IRemarkableDevice _remarkableDevice;
    private readonly INotificationService _notificationService;
    private readonly PathList _pathList;

    public RestoreTool(IPathManager pathManager,
                       IRemarkableDevice remarkableDevice,
                       INotificationService notificationService,
                       PathList pathList)
    {
        _pathManager = pathManager;
        _remarkableDevice = remarkableDevice;
        _notificationService = notificationService;
        _pathList = pathList;
    }

    public IImage Image
    {
        get
        {
            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

            return new Bitmap(assets.Open(new Uri("avares://Slithin.Modules.Backup/Resources/restore.png")));
        }
    }

    public ToolInfo Info => new("restore", "Restore", "Internal", "Restore a Backup", false, true);

    public bool IsConfigurable => false;

    public Control GetModal()
    {
        return null;
    }

    public async void Invoke(object data)
    {
        var vm = new SelectBackupViewModel
        {
            Backups = new ObservableCollection<Backup>(Directory.GetFiles(_pathManager.BackupsDir, "*.zip")
                .Except(Directory.GetFiles(_pathManager.BackupsDir, "*.notebook.zip"))
                .Select(_ => new Backup(Path.GetFileNameWithoutExtension(_).Replace("Backup_from_", ""), _)))
        };

        var result = true; // await DialogService.ShowDialog("Select Backup", new SelectBackupModal { DataContext = vm });

        if (result)
        {
            var really = true; // await DialogService.ShowDialog(
                               //$"Do you really want to restore backup {vm.SelectedBackup.Name}? All files will be replaced!");

            if (really)
            {
                _notificationService.Show("Restoring Backup");

                //delete local files but not backups folder
                _notificationService.Show("Deleting Local Files - Screens");
                Directory.Delete(_pathManager.CustomScreensDir, true);

                _notificationService.Show("Deleting Local Files - Notebooks");
                Directory.Delete(_pathManager.NotebooksDir, true);

                _notificationService.Show("Deleting Local Files - Templates");
                Directory.Delete(_pathManager.TemplatesDir, true);

                //extract zip
                _notificationService.Show("Extracting Backup");
                using (var zip = new ZipFile(vm.SelectedBackup.Filename))
                {
                    zip.ExtractAll(_pathManager.ConfigBaseDir, ExtractExistingFileAction.OverwriteSilently);
                }

                //upload all data
                _notificationService.Show("Removing Notebooks From Device");
                _remarkableDevice.RunCommand("rm -fr " + _pathList.Documents);
                _remarkableDevice.RunCommand("mkdir " + _pathList.Documents);

                _notificationService.Show("Removing Screens From Device");
                _remarkableDevice.RunCommand("rm -fr " + _pathList.Screens);
                _remarkableDevice.RunCommand("mkdir " + _pathList.Screens);

                _notificationService.Show("Removing Templates From Device");
                _remarkableDevice.RunCommand("rm -fr " + _pathList.Templates);
                _remarkableDevice.RunCommand("mkdir " + _pathList.Templates);

                _notificationService.Show("Uploading Notebooks");
                _remarkableDevice.Upload(new DirectoryInfo(_pathManager.NotebooksDir), _pathList.Documents);

                _notificationService.Show("Uploading Screens");
                _remarkableDevice.Upload(new DirectoryInfo(_pathManager.CustomScreensDir), _pathList.Screens);

                _notificationService.Show("Uploading Templates");
                _remarkableDevice.Upload(new DirectoryInfo(_pathManager.TemplatesDir), _pathList.Templates);

                _notificationService.Show("Finished");
                await Task.Delay(1000);

                _remarkableDevice.Reload();
            }
        }
    }
}
