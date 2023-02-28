using AuroraModularis.Core;
using Slithin.Controls;
using Slithin.Modules.BaseServices.Models;
using Slithin.Modules.Device.Models;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.Repository.Models;
using Slithin.Modules.Sync.Models;
using Slithin.Modules.UI.Models;

namespace Slithin.Modules.Sync;

public class SynchronizeImpl : ISynchronizeService
{
    public async Task Synchronize(bool notificationsInNewWindow)
    {
        var device = ServiceContainer.Current.Resolve<IRemarkableDevice>();
        var pathManager = ServiceContainer.Current.Resolve<IPathManager>();
        var notificationService = ServiceContainer.Current.Resolve<INotificationService>();
        var locService = ServiceContainer.Current.Resolve<ILocalisationService>();
        var mdService = ServiceContainer.Current.Resolve<IMetadataRepository>();

        using var status = notificationService.ShowStatus(locService.GetString("Synchronizing Device: Fetching Notebooks"),
            isCancellable: true,
            notificationsInNewWindow);
        
        if (SynchronizeNotebooks(device, status, mdService, pathManager, locService))
        {
            return;
        }

        if (SynhronizeTemplates(status, locService, device, pathManager))
        {
            return;
        }

        if (SynchronizeScreens(status, locService, device, pathManager))
        {
            return;
        }

        status.Step(locService.GetString("Finish"));
        
        await Task.Delay(1000);
    }

    private static bool SynchronizeScreens(IStatusController status, ILocalisationService locService,
        IRemarkableDevice device, IPathManager pathManager)
    {
        status.Step(locService.GetString("Synchronizing Device: Fetching Screens"));
        var screens = device.FetchedScreens;

        foreach (var fetchResult in screens)
        {
            if (status.Token.IsCancellationRequested)
            {
                return true;
            }

            var path = Path.Combine(pathManager.CustomScreensDir, fetchResult.ShortPath);
            var fileInfo = new FileInfo(path);
            if (!fileInfo.Exists || IsFileOlder(fileInfo, fetchResult.LastModified))
            {
                status.Step(locService.GetStringFormat("Sync Screen: Downloading {0} ...", fetchResult.ShortPath));
                device.Download(fetchResult.FullPath, fileInfo);
            }
            else
            {
                status.Step(locService.GetStringFormat("Sync Screen: Skipping {0} (Up to date)", fetchResult.ShortPath));
            }
        }

        return false;
    }

    private static bool SynhronizeTemplates(IStatusController status, ILocalisationService locService,
        IRemarkableDevice device, IPathManager pathManager)
    {
        status.Step(locService.GetString("Synchronizing Device: Fetching Templates"));
        var templates = device.FetchedTemplates;

        foreach (var fetchResult in templates)
        {
            if (status.Token.IsCancellationRequested)
            {
                return true;
            }

            var path = fetchResult.ShortPath == "templates.json"
                ? Path.Combine(pathManager.ConfigBaseDir, "templates.json")
                : Path.Combine(pathManager.TemplatesDir, fetchResult.ShortPath);
            var fileInfo = new FileInfo(path);
            if (!fileInfo.Exists || IsFileOlder(fileInfo, fetchResult.LastModified))
            {
                status.Step(locService.GetStringFormat("Sync Template: Downloading {0} ...", fetchResult.ShortPath));
                device.Download(fetchResult.FullPath, fileInfo);
            }
            else
            {
                status.Step(locService.GetStringFormat("Sync Template: Skipping {0} (Up to date)", fetchResult.ShortPath));
            }
        }

        return false;
    }

    private static bool SynchronizeNotebooks(IRemarkableDevice device, IStatusController status,
        IMetadataRepository mdService, IPathManager pathManager, ILocalisationService locService)
    {
        var notebooks = device.FetchedNotebooks;
        var groupedNotebooks = notebooks.GroupBy(k => Path.GetFileNameWithoutExtension(k.ShortPath)).ToArray();

        for (int i = 0; i < groupedNotebooks.Length; i++)
        {
            status.Step(locService.GetStringFormat("Downloading {0} / {1}", i, groupedNotebooks.Count()));

            foreach (var notebook in groupedNotebooks)
            {
                foreach (var fetchResult in notebook)
                {
                    var fetchresultWithResolvedName =
                        fetchResult with { ShortPath = mdService.Load(notebook.Key).VisibleName };

                    if (DownloadNotebookFile(status, pathManager, fetchresultWithResolvedName, locService, device))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private static bool DownloadNotebookFile(IStatusController status, IPathManager pathManager,
        FileFetchResult fetchResult, ILocalisationService locService, IRemarkableDevice device)
    {
        if (status.Token.IsCancellationRequested)
        {
                return true;
        }

        var path = Path.Combine(pathManager.NotebooksDir, fetchResult.ShortPath);
        var fileInfo = new FileInfo(path);
        if (!fileInfo.Exists || IsFileOlder(fileInfo, fetchResult.LastModified))
        {
            status.Step(locService.GetStringFormat("Sync Notebook: Downloading {0} ...", fetchResult.ShortPath));
            device.Download(fetchResult.FullPath, fileInfo);
        }
        else
        {
            status.Step(locService.GetStringFormat("Sync Notebook: Skipping {0} (Up to date)", fetchResult.ShortPath));
        }

        return false;
    }

    private static bool IsFileOlder(FileInfo file, long other)
    {
        var time = file.LastWriteTime.Ticks;
        return time < other;
    }
}
