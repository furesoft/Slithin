using AuroraModularis.Core;
using Slithin.Modules.BaseServices.Models;
using Slithin.Modules.Device.Models;
using Slithin.Modules.Sync.Models;
using Slithin.Modules.UI.Models;

namespace Slithin.Modules.Sync;

public class SynchronizeImpl : ISynchronizeService
{
    public async void Synchronize()
    {
        await Task.Run(() =>
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
                        status.Step($"Sync Notebook: {p} [Downloading...]");
                        device.Download($"/home/root/.local/share/remarkable/xochitl/{p}", fileInfo);
                    }
                    else
                    {
                        status.Step($"Sync Notebook: {p} [Up to date]");
                    }
                }
            }

            notificationService.ShowStatus("Synchronizing Device: Fetching Templates");

            var templates = device.FetchFilesWithModified("/usr/share/remarkable/templates");
        });
    }

    private static bool IsFileOlder(FileInfo file, long other)
    {
        var time = file.LastWriteTime.Ticks;
        return time < other;
    }
}
