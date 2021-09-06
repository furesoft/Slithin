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

        public void HandleMessage(CollectSyncNotebooksMessage message)
        {
            var notebooksDir = _pathManager.NotebooksDir;

            NotificationService.Show("Collecting Filenames");

            var cmd = _client.RunCommand("ls -p " + PathList.Documents);
            var allFilenames = cmd.Result.Split('\n', StringSplitOptions.RemoveEmptyEntries).Where(_ => !_.EndsWith(".zip") && !_.EndsWith(".zip.part"));
            var mds = new List<Metadata>();
            var mdFilenames = allFilenames.Where(_ => _.EndsWith(".metadata")).ToArray();
            var mdLocals = new Dictionary<string, Metadata>();

            var thumbnailFolders = allFilenames.Where(_ => _.EndsWith(".thumbnails/"));
            var thumbnailFoldersToSync = thumbnailFolders.Where(_ => !Directory.Exists(Path.Combine(notebooksDir, _.Substring(0, _.Length - 1))));

            var thumbnailsSync = new SyncNotebook();
            thumbnailsSync.Directories = thumbnailFoldersToSync;

            if (thumbnailFolders.Any())
            {
                _syncNotebooks.Add(thumbnailsSync);
            }

            for (var i = 0; i < mdFilenames.Length; i++)
            {
                var md = mdFilenames[i];
                NotificationService.Show($"Downloading Notebook Metadata {i} / {mdFilenames.Length}");

                var mdContent = _client.RunCommand("cat " + PathList.Documents + "/" + md).Result;
                var contentContent = "{}";
                var pageDataContent = "";

                if (allFilenames.Contains(Path.ChangeExtension(md, ".content")))
                {
                    contentContent = _client.RunCommand("cat " + PathList.Documents + "/" + Path.ChangeExtension(md, ".content")).Result;
                }
                if (allFilenames.Contains(Path.ChangeExtension(md, ".pagedata")))
                {
                    pageDataContent = _client.RunCommand("cat " + PathList.Documents + "/" + Path.ChangeExtension(md, ".pagedata")).Result;
                }

                if (string.IsNullOrEmpty(mdContent))
                {
                    continue;
                }

                var mdObj = JsonConvert.DeserializeObject<Metadata>(mdContent);

                var contentObj = JsonConvert.DeserializeObject<ContentFile>(contentContent);
                Metadata mdLocalObj;

                if (File.Exists(Path.Combine(notebooksDir, md)))
                {
                    mdLocalObj = JsonConvert.DeserializeObject<Metadata>(File.ReadAllText(Path.Combine(notebooksDir, md)));
                }
                else
                {
                    mdLocalObj = new();
                    mdLocalObj.Version = 0;
                }

                mdObj.ID = Path.GetFileNameWithoutExtension(md);
                mdObj.Content = contentObj;
                mdObj.PageData.Parse(pageDataContent);

                mdLocals.Add(mdObj.ID, mdLocalObj);

                if (File.Exists(Path.Combine(notebooksDir, md)))
                {
                    if (!mdObj.Deleted)
                    {
                        if (mdObj.Version > mdLocalObj.Version)
                        {
                            if (mdObj.Type == "DocumentType")
                            {
                                mds.Add(mdObj);
                            }

                            File.WriteAllText(Path.Combine(notebooksDir, md), mdContent);

                            if (allFilenames.Contains(Path.ChangeExtension(md, ".content")))
                            {
                                File.WriteAllText(Path.Combine(notebooksDir, Path.ChangeExtension(md, ".content")), contentContent);
                            }
                            if (allFilenames.Contains(Path.ChangeExtension(md, ".pageData")))
                            {
                                File.WriteAllText(Path.Combine(notebooksDir, Path.ChangeExtension(md, ".pageData")), pageDataContent);
                            }
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

                    if (allFilenames.Contains(Path.ChangeExtension(md, ".content")))
                    {
                        File.WriteAllText(Path.Combine(notebooksDir, Path.ChangeExtension(md, ".content")), contentContent);
                    }
                    if (allFilenames.Contains(Path.ChangeExtension(md, ".pageData")))
                    {
                        File.WriteAllText(Path.Combine(notebooksDir, Path.ChangeExtension(md, ".pageData")), pageDataContent);
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
                        if (!md.Deleted)
                        {
                            if (md.Version > mdLocals[md.ID].Version)
                            {
                                if (!fi.Exists)
                                {
                                    _syncNotebooks.Add(sn);
                                }
                            }
                        }
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
