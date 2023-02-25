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
            var notebooks = device.FetchFilesWithModified("/home/root/.local/share/remarkable/xochitl");

            foreach (var (p, time) in notebooks)
            {
                var path = Path.Combine(pathManager.NotebooksDir, p);
                var fileInfo = new FileInfo(path);
                if (!fileInfo.Exists || IsFileOlder(fileInfo, time))
                {
                    status.Step($"Sync Notebook: Downloading {p} ...");
                    device.Download($"/home/root/.local/share/remarkable/xochitl/{p}", fileInfo);
                }
                else
                {
                    status.Step($"Sync Notebook: Skipping {p} (Up to date)");
                }
            }
        }

        {
            var status = notificationService.ShowStatus("Synchronizing Device: Fetching Templates");
            var templates = device.FetchFilesWithModified("/usr/share/remarkable/templates");

            foreach (var (p, time) in templates)
            {
                var path = p == "templates.json" ? Path.Combine(pathManager.ConfigBaseDir, "templates.json") : Path.Combine(pathManager.TemplatesDir, p);
                var fileInfo = new FileInfo(path);
                if (!fileInfo.Exists || IsFileOlder(fileInfo, time))
                {
                    status.Step($"Sync Template: Downloading {p} ...");
                    device.Download($"/usr/share/remarkable/templates/{p}", fileInfo);
                }
                else
                {
                    status.Step($"Sync Template: Skipping {p} (Up to date)");
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
