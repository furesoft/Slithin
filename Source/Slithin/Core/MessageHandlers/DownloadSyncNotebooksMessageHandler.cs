using System.IO;
using Renci.SshNet;
using Slithin.Core.Services;
using Slithin.Messages;

namespace Slithin.Core.MessageHandlers
{
    public class DownloadSyncNotebooksMessageHandler : IMessageHandler<DownloadSyncNotebookMessage>
    {
        private readonly IPathManager _pathManager;
        private readonly ScpClient _scpClient;

        public DownloadSyncNotebooksMessageHandler(IPathManager pathManager, ScpClient scpClient)
        {
            _pathManager = pathManager;
            _scpClient = scpClient;
        }

        public void HandleMessage(DownloadSyncNotebookMessage message)
        {
            for (int i = 0; i < message.Notebooks.Count; i++)
            {
                NotificationService.Show($"Downloading Notebook {i + 1}/{message.Notebooks.Count}");

                var sn = message.Notebooks[i];

                foreach (var folder in sn.Directories)
                {
                    var di = new DirectoryInfo(Path.Combine(_pathManager.NotebooksDir, folder));

                    if (!di.Exists)
                    {
                        di.Create();
                    }

                    _scpClient.Download(PathList.Documents + "/" + folder, di);
                }

                foreach (var file in sn.Files)
                {
                    var fi = new FileInfo(Path.Combine(_pathManager.NotebooksDir, file));

                    _scpClient.Download(PathList.Documents + "/" + file, fi);
                }

                NotificationService.Hide();
            }
        }
    }
}
