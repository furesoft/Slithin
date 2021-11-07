using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Renci.SshNet;
using Slithin.Core.Remarkable;
using Slithin.Core.Services;
using Slithin.Core.Sync;
using Slithin.Messages;
using Slithin.Models;

namespace Slithin.Core.MessageHandlers
{
    public class CollectSyncNotebooksMessageHandler : IMessageHandler<CollectSyncNotebooksMessage>
    {
        private readonly SshClient _client;
        private readonly IMailboxService _mailboxService;
        private readonly IPathManager _pathManager;
        private readonly ScpClient _scp;
        private readonly SynchronisationService _synchronisationService;
        private readonly List<SyncNotebook> _syncNotebooks = new();

        public CollectSyncNotebooksMessageHandler(IPathManager pathManager,
                                               SshClient client,
                                               ScpClient scp,
                                               IMailboxService mailboxService)
        {
            _pathManager = pathManager;
            _client = client;
            _scp = scp;
            _mailboxService = mailboxService;
            _synchronisationService = ServiceLocator.SyncService;
        }

        public void HandleMessage(CollectSyncNotebooksMessage message) //Maybe refactor to multiple smaller Methods?
        {
            var notebooksDir = _pathManager.NotebooksDir;

            NotificationService.Show("Collecting Filenames");

            var cmd = _client.RunCommand("ls -p " + PathList.Documents);
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
                .Where(x => !Directory.Exists(Path.Combine(notebooksDir, x[0..^1])));

            var thumbnailsSync = new SyncNotebook
            {
                Directories = thumbnailFoldersToSync
            };

            if (thumbnailFolders.Any())
            {
                _syncNotebooks.Add(thumbnailsSync);
            }

            for (var i = 0; i < mdFilenames.Length; i++)
            {
                var md = mdFilenames[i];
                NotificationService.Show($"Downloading Notebook Metadata {i} / {mdFilenames.Length}");

                var sshCommand = _client.RunCommand("cat " + PathList.Documents + "/" + md);
                var mdContent = sshCommand.Result;
                var contentContent = "{}";
                var pageDataContent = "";

                var mdDotContent = Path.ChangeExtension(md, ".content");
                var fileNamesContainDotContent = allFilenames.Contains(mdDotContent);
                if (fileNamesContainDotContent)
                {
                    contentContent = _client.RunCommand("cat " + PathList.Documents + "/" + mdDotContent).Result;
                }
                var mdDotPagedata = Path.ChangeExtension(md, ".pagedata");
                var fileNamesContaintDotPagedata = allFilenames.Contains(mdDotPagedata);
                if (fileNamesContaintDotPagedata)
                {
                    pageDataContent = _client.RunCommand("cat " + PathList.Documents + "/" + mdDotPagedata).Result;
                }

                if (string.IsNullOrEmpty(mdContent) || string.IsNullOrWhiteSpace(mdContent))
                {
                    continue;
                }

                var mdObj = JsonConvert.DeserializeObject<Metadata>(mdContent);

                var contentObj = JsonConvert.DeserializeObject<ContentFile>(contentContent);
                var mdLocalObj
                    = File.Exists(Path.Combine(notebooksDir, md))
                    ? JsonConvert.DeserializeObject<Metadata>(File.ReadAllText(Path.Combine(notebooksDir, md)))
                    : (new() { Version = 0 });

                mdObj.ID = Path.GetFileNameWithoutExtension(md);
                mdObj.Content = contentObj;
                mdObj.PageData.Parse(pageDataContent);

                mdLocals.Add(mdObj.ID, mdLocalObj);

                if (File.Exists(Path.Combine(notebooksDir, md)))
                {
                    if (!mdObj.Deleted && mdObj.Version > mdLocalObj.Version || mdObj.Parent != mdLocalObj.Parent)
                    {
                        if (mdObj.Type == "DocumentType")
                            mds.Add(mdObj);
                        File.WriteAllText(Path.Combine(notebooksDir, md), mdContent);

                        if (fileNamesContainDotContent)
                            File.WriteAllText(Path.Combine(notebooksDir, mdDotContent), contentContent);
                        if (fileNamesContaintDotPagedata)
                            File.WriteAllText(Path.Combine(notebooksDir, mdDotPagedata), pageDataContent);
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

                if (mdObj.Type == "CollectionType" && mdObj.Parent == "")
                {
                    MetadataStorage.Local.Add(mdObj, out var alreadyAdded);

                    if (!alreadyAdded)
                    {
                        _synchronisationService.NotebooksFilter.Documents.Add(mdObj);
                    }
                }
            }

            foreach (var md in mds)
            {
                SyncNotebook sn = new();
                sn.Metadata = md;

                if (md.Content.FileType == "notebook")
                {
                    var allFolders = allFilenames.Where(_ => _.StartsWith(md.ID) && _.EndsWith("/"));

                    sn.Directories = allFolders;

                    _syncNotebooks.Add(sn);
                }
                else
                {
                    var otherfiles = allFilenames.Where(_ => !_.EndsWith(".metadata") && !_.EndsWith("/") && _.StartsWith(md.ID)).ToArray();

                    sn.Files = otherfiles;

                    for (var i = 0; i < otherfiles.Length; i++)
                    {
                        var fi = new FileInfo(Path.Combine(notebooksDir, otherfiles[i]));
                        if (!md.Deleted
                            && md.Version > mdLocals[md.ID].Version
                            && !fi.Exists)
                            _syncNotebooks.Add(sn);
                    }
                }

                MetadataStorage.Local.Add(md, out var alreadyAdded);

                if (md.Parent == "" && !alreadyAdded)
                {
                    _synchronisationService.NotebooksFilter.Documents.Add(md);
                }
            }

            NotificationService.Hide();

            _mailboxService.Post(new DownloadSyncNotebookMessage(_syncNotebooks));
        }
    }
}