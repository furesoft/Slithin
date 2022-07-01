using System.IO;
using Slithin.Core;
using Slithin.Core.Messaging;
using Slithin.Core.Remarkable;
using Slithin.Core.Services;
using Slithin.Messages;

namespace Slithin.MessageHandlers;

public class DownloadSyncNotebooksMessageHandler : IMessageHandler<DownloadSyncNotebookMessage>
{
    private readonly ILocalisationService _localisationService;
    private readonly IPathManager _pathManager;
    private readonly ISSHService _ssh;

    public DownloadSyncNotebooksMessageHandler(IPathManager pathManager,
                                               ILocalisationService localisationService,
                                               ISSHService ssh)
    {
        _pathManager = pathManager;
        _localisationService = localisationService;
        _ssh = ssh;
    }

    public void HandleMessage(DownloadSyncNotebookMessage message)
    {
        for (int i = 0; i < message.Notebooks.Count; i++)
        {
            var sn = message.Notebooks[i];

            foreach (var folder in sn.Directories)
            {
                var di = new DirectoryInfo(Path.Combine(_pathManager.NotebooksDir, folder.TrimStart('/')));

                if (!di.Exists)
                {
                    di.Create();
                }

                _ssh.Downloading += (s, e) =>
                {
                    NotificationService.ShowProgress(_localisationService.GetStringFormat(
                        "Downloading Notebook"), (int)e.Downloaded, (int)e.Size);
                };

                _ssh.Download(PathList.Documents + folder.TrimStart('/'), di);
            }

            foreach (var file in sn.Files)
            {
                var fi = new FileInfo(Path.Combine(_pathManager.NotebooksDir, file));

                _ssh.Download(PathList.Documents + "/" + file, fi);
            }
        }
    }
}
