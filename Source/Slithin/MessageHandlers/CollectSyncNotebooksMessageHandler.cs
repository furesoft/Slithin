using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Renci.SshNet;
using Slithin.Core;
using Slithin.Core.Messaging;
using Slithin.Core.Remarkable;
using Slithin.Core.Remarkable.Models;
using Slithin.Core.Services;
using Slithin.Core.Sync;
using Slithin.Messages;
using Slithin.Models;

namespace Slithin.MessageHandlers;

public class CollectSyncNotebooksMessageHandler : IMessageHandler<CollectSyncNotebooksMessage>
{
    private readonly SshClient _client;
    private readonly ILocalisationService _localisationService;
    private readonly IMailboxService _mailboxService;
    private readonly IPathManager _pathManager;
    private readonly SynchronisationService _synchronisationService;
    private readonly List<SyncNotebook> _syncNotebooks = new();

    public CollectSyncNotebooksMessageHandler(IPathManager pathManager,
        SshClient client,
        ILocalisationService localisationService,
        IMailboxService mailboxService)
    {
        _pathManager = pathManager;
        _client = client;
        _localisationService = localisationService;
        _mailboxService = mailboxService;
        _synchronisationService = ServiceLocator.SyncService;
    }

    public void HandleMessage(CollectSyncNotebooksMessage message)
    {
        var notebooksDir = _pathManager.NotebooksDir;

        var cmd = _client.RunCommand($"ls -p {PathList.Documents}");
        var allFilenames
            = cmd.Result
                .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Where(f => !f.EndsWith(".zip") && !f.EndsWith(".zip.part"))
                .ToList(); //Because multiple iterations are happening, so lazy loading is not needed

        var mds = new List<Metadata>();
        var mdFilenames
            = allFilenames
                .Where(x => x.EndsWith(".metadata"))
                .ToArray();

        var mdLocals = new Dictionary<string, Metadata>();

        var thumbnailFolders
            = allFilenames
                .Where(x => x.EndsWith(".thumbnails/"));

        var thumbnailFoldersToSync
            = thumbnailFolders
                .Where(x => !Directory.Exists(Path.Combine(notebooksDir, x[..^1])));


        if (thumbnailFoldersToSync.Any())
        {
        var thumbnailsSync = new SyncNotebook { Directories = thumbnailFoldersToSync };
            _syncNotebooks.Add(thumbnailsSync);
        }

        int currentMd = 0;
        Parallel.For(0, mdFilenames.Length, (i, x) =>
        {
            var md = mdFilenames[i];
            NotificationService.ShowProgress(
                _localisationService.GetString(
                    "Downloading Notebook Metadata"), currentMd, mdFilenames.Length - 1);
            currentMd++;

            var sshCommand = _client.RunCommand($"cat {PathList.Documents}/{md}");
            var mdContent = sshCommand.Result;
            var contentContent = "{}";
            var pageDataContent = "";

            var mdDotContent = Path.ChangeExtension(md, ".content");
            var fileNamesContainDotContent = allFilenames.Contains(mdDotContent);
            if (fileNamesContainDotContent)
            {
                contentContent = _client.RunCommand($"cat {PathList.Documents}/{mdDotContent}").Result;
            }

            var mdDotPagedata = Path.ChangeExtension(md, ".pagedata");
            var fileNamesContaintDotPagedata = allFilenames.Contains(mdDotPagedata);
            if (fileNamesContaintDotPagedata)
            {
                pageDataContent = _client.RunCommand($"cat {PathList.Documents}/{mdDotPagedata}").Result;
            }

            if (string.IsNullOrEmpty(mdContent) || string.IsNullOrWhiteSpace(mdContent))
            {
                return;
            }

            var mdObj = JsonConvert.DeserializeObject<Metadata>(mdContent);

            var contentObj = JsonConvert.DeserializeObject<ContentFile>(contentContent);
            var mdLocalObj
                = File.Exists(Path.Combine(notebooksDir, md))
                    ? JsonConvert.DeserializeObject<Metadata>(File.ReadAllText(Path.Combine(notebooksDir, md)))
                    : new Metadata { Version = 0 };

            InitMetadata(mdObj, md, contentObj, pageDataContent, mdLocals, mdLocalObj);

            SaveMetadata(notebooksDir, md, mdObj, mdLocalObj, mds, mdContent, fileNamesContainDotContent, mdDotContent,
                contentContent, fileNamesContaintDotPagedata, mdDotPagedata, pageDataContent);
        });

        ConvertMetadataToSyncNotebook(mds, allFilenames, notebooksDir, mdLocals);

        _mailboxService.Post(new DownloadSyncNotebookMessage(_syncNotebooks));
    }

    private static void InitMetadata(Metadata mdObj, string md, ContentFile contentObj, string pageDataContent,
        Dictionary<string, Metadata> mdLocals, Metadata mdLocalObj)
    {
        mdObj.ID = Path.GetFileNameWithoutExtension(md);
        mdObj.Content = contentObj;
        mdObj.PageData.Parse(pageDataContent);

        mdLocals.Add(mdObj.ID, mdLocalObj);
    }

    private void ConvertMetadataToSyncNotebook(List<Metadata> mds, List<string> allFilenames, string notebooksDir,
            Dictionary<string, Metadata> mdLocals)
    {
        foreach (var md in mds)
        {
            SyncNotebook sn = new() { Metadata = md };

            if (md.Content.FileType == "notebook")
            {
                var allFolders = allFilenames.Where(_ => _.StartsWith(md.ID) && _.EndsWith("/"));

                sn.Directories = allFolders;

                _syncNotebooks.Add(sn);
            }
            else
            {
                var otherfiles = allFilenames
                    .Where(_ => !_.EndsWith(".metadata") && !_.EndsWith("/") && _.StartsWith(md.ID)).ToArray();

                sn.Files = otherfiles;

                for (var i = 0; i < otherfiles.Length; i++)
                {
                    var fi = new FileInfo(Path.Combine(notebooksDir, otherfiles[i]));
                    if (!md.Deleted
                        && md.Version > mdLocals[md.ID].Version
                        && !fi.Exists)
                    {
                        _syncNotebooks.Add(sn);
                    }
                }
            }

            MetadataStorage.Local.AddMetadata(md, out var alreadyAdded);

            if (md.Parent == "" && !_synchronisationService.NotebooksFilter.Documents.Contains(md) && !alreadyAdded)
            {
                _synchronisationService.NotebooksFilter.Documents.Add(md);
            }
        }
    }

    private void SaveMetadata(string notebooksDir, string md, Metadata mdObj, Metadata mdLocalObj, List<Metadata> mds,
        string mdContent, bool fileNamesContainDotContent, string mdDotContent, string contentContent,
        bool fileNamesContaintDotPagedata, string mdDotPagedata, string pageDataContent)
    {
        if (File.Exists(Path.Combine(notebooksDir, md)))
        {
            if (!mdObj.Deleted && mdObj.Version > mdLocalObj.Version || mdObj.Parent != mdLocalObj.Parent)
            {
                if (mdObj.Type == "DocumentType")
                {
                    mds.Add(mdObj);
                }

                File.WriteAllText(Path.Combine(notebooksDir, md), mdContent);

                if (fileNamesContainDotContent)
                {
                    File.WriteAllText(Path.Combine(notebooksDir, mdDotContent), contentContent);
                }

                if (fileNamesContaintDotPagedata)
                {
                    File.WriteAllText(Path.Combine(notebooksDir, mdDotPagedata), pageDataContent);
                }
            }
        }
        else
        {
            if (mdObj.Type == "DocumentType")
            {
                mds.Add(mdObj);
            }

            File.WriteAllText(Path.Combine(notebooksDir, md), mdContent);

            if (fileNamesContainDotContent)
            {
                File.WriteAllText(Path.Combine(notebooksDir, mdDotContent), contentContent);
            }

            if (fileNamesContaintDotPagedata)
            {
                File.WriteAllText(Path.Combine(notebooksDir, mdDotPagedata), pageDataContent);
            }
        }

        if (mdObj.Type == "CollectionType")
        {
            MetadataStorage.Local.AddMetadata(mdObj, out var alreadyAdded);

            if (!alreadyAdded)
            {
                if (!_synchronisationService.NotebooksFilter.Documents.Contains(mdObj) && mdObj.Parent == "")
                {
                    _synchronisationService.NotebooksFilter.Documents.Add(mdObj);
                }
            }
        }
    }
}
