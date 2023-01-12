using System.IO;

namespace Slithin.MessageHandlers;

public class DownloadSyncNotebooksMessageHandler : IMessageHandler<DownloadSyncNotebookMessage>
{
    private readonly ILocalisationService _localisationService;
    private readonly IPathManager _pathManager;

    public DownloadSyncNotebooksMessageHandler(IPathManager pathManager,
                                               ILocalisationService localisationService)
    {
        _pathManager = pathManager;
        _localisationService = localisationService;
    }

    public void HandleMessage(DownloadSyncNotebookMessage message)
    {
        var ssh = ServiceLocator.Container.Resolve<ISSHService>();

        ssh.Downloading += (s, e) =>
        {
            NotificationService.ShowProgress(_localisationService.GetStringFormat(
                "Downloading Notebook"), (int)e.Downloaded, (int)e.Size);
        };

        var snn = new SyncNotebook[message.Notebooks.Count];
        message.Notebooks.CopyTo(snn, 0);

        for (int i = 0; i < message.Notebooks.Count; i++)
        {
            var sn = snn[i];

            foreach (var folder in sn.Directories)
            {
                var di = new DirectoryInfo(Path.Combine(_pathManager.NotebooksDir, folder.TrimStart('/')));

                if (!di.Exists)
                {
                    di.Create();
                }

                ssh.Download(PathList.Documents + folder.TrimStart('/'), di);
            }

            foreach (var file in sn.Files)
            {
                var fi = new FileInfo(Path.Combine(_pathManager.NotebooksDir, file));

                ssh.Download(PathList.Documents + "/" + file, fi);
            }
        }
    }
}
