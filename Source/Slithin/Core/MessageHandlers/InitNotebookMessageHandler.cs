using System;
using System.IO;
using System.Linq;
using Renci.SshNet;
using Renci.SshNet.Common;
using Slithin.Core.Services;
using Slithin.Messages;

namespace Slithin.Core.MessageHandlers;

public class InitNotebookMessageHandler : IMessageHandler<InitNotebookMessage>
{
    private readonly SshClient _client;
    private readonly ILocalisationService _localisationService;
    private readonly IPathManager _pathManager;
    private readonly ScpClient _scp;

    public InitNotebookMessageHandler(ScpClient scp,
        IPathManager pathManager,
        ILocalisationService localisationService,
        SshClient client)
    {
        _scp = scp;
        _pathManager = pathManager;
        _localisationService = localisationService;
        _client = client;
    }

    public void HandleMessage(InitNotebookMessage message)
    {
        var notebooksDir = _pathManager.NotebooksDir;
        NotificationService.Show(_localisationService.GetString("Downloading Notebooks"));

        var cmd = _client.RunCommand("ls -p " + PathList.Documents);
        var allNodes
            = cmd.Result
                .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Where(_ => !_.EndsWith(".zip") && !_.EndsWith(".zip.part"));

        _scp.Downloading += OnDownloading;

        foreach (var node in allNodes)
        {
            if (!node.EndsWith("/"))
            {
                _scp.Download(PathList.Documents + "/" + node, new FileInfo(Path.Combine(notebooksDir, node)));
                break;
            }

            Directory.CreateDirectory(Path.Combine(notebooksDir, node.Remove(node.Length - 1, 1)));
            _scp.Download(PathList.Documents + "/" + node, new DirectoryInfo(Path.Combine(notebooksDir, node.Remove(node.Length - 1, 1))));
        }

        _scp.Downloading -= OnDownloading;
    }

    private void OnDownloading(object sender, ScpDownloadEventArgs e)
    {
        NotificationService.Show(_localisationService.GetStringFormat(
            "Downloading {0}", "{e.Filename} {e.Downloaded:n0} Bytes/ {e.Size:n0} Bytes"));
    }
}
