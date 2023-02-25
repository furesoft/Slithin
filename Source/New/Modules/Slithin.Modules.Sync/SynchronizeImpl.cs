using AuroraModularis.Core;
using Slithin.Modules.BaseServices.Models;
using Slithin.Modules.Device.Models;
using Slithin.Modules.Sync.Models;
using Slithin.Modules.UI.Models;

namespace Slithin.Modules.Sync;

public class SynchronizeImpl : ISynchronizeService
{
    public Task Synchronize()
    {
        var device = ServiceContainer.Current.Resolve<IRemarkableDevice>();
        var pathManager = ServiceContainer.Current.Resolve<IPathManager>();
        var notificationService = ServiceContainer.Current.Resolve<INotificationService>();

        {
            var status = notificationService.ShowStatus("Synchronizing Device: Fetching Notebooks");
            var notebooks = device.FetchedNotebooks;

            foreach (var fetchResult in notebooks)
            {
                var path = Path.Combine(pathManager.NotebooksDir, fetchResult.ShortPath);
                var fileInfo = new FileInfo(path);
                if (!fileInfo.Exists || IsFileOlder(fileInfo, fetchResult.LastModified))
                {
                    status.Step($"Sync Notebook: Downloading {fetchResult.ShortPath} ...");
                    device.Download(fetchResult.FullPath, fileInfo);
                }
                else
                {
                    status.Step($"Sync Notebook: Skipping {fetchResult.ShortPath} (Up to date)");
                }
            }
        }

        {
            var status = notificationService.ShowStatus("Synchronizing Device: Fetching Templates");
            var templates = device.FetchedTemplates;

            foreach (var fetchResult in templates)
            {
                var path = fetchResult.ShortPath == "templates.json" ? Path.Combine(pathManager.ConfigBaseDir, "templates.json") : Path.Combine(pathManager.TemplatesDir, fetchResult.ShortPath);
                var fileInfo = new FileInfo(path);
                if (!fileInfo.Exists || IsFileOlder(fileInfo, fetchResult.LastModified))
                {
                    status.Step($"Sync Template: Downloading {fetchResult.ShortPath} ...");
                    device.Download(fetchResult.FullPath, fileInfo);
                }
                else
                {
                    status.Step($"Sync Template: Skipping {fetchResult.ShortPath} (Up to date)");
                }
            }
        }

        return Task.CompletedTask;
    }

    private static bool IsFileOlder(FileInfo file, long other)
    {
        var time = file.LastWriteTime.Ticks;
        return time < other;
    }
}
