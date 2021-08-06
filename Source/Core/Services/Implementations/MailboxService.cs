using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Actress;
using Newtonsoft.Json;
using Renci.SshNet;
using Renci.SshNet.Common;
using Slithin.Controls;
using Slithin.Core.Remarkable;
using Slithin.Core.Sync;
using Slithin.Core.Sync.Repositorys;
using Slithin.Messages;

namespace Slithin.Core.Services.Implementations
{
    public class MailboxService : IMailboxService
    {
        private readonly DeviceRepository _device;
        private readonly LocalRepository _local;
        private readonly IPathManager _pathManager;
        private readonly SynchronisationService _syncService;
        private MailboxProcessor<AsynchronousMessage> _mailbox;

        public MailboxService(IPathManager pathManager,
            SynchronisationService syncService, DeviceRepository device, LocalRepository local)
        {
            _pathManager = pathManager;
            _syncService = syncService;
            _device = device;
            _local = local;
        }

        public void Init()
        {
            _mailbox = MailboxProcessor.Start<AsynchronousMessage>(
                async (_) =>
                {
                    while (true)
                    {
                        var msg = await _.Receive();

                        MessageRouter.Route(msg);
                    }
                }
            );
        }

        public void InitMessageRouter()
        {
            var client = ServiceLocator.Container.Resolve<SshClient>();
            var scp = ServiceLocator.Container.Resolve<ScpClient>();

            MessageRouter.Register<SyncMessage>(_ =>
            {
                var notebooksDir = _pathManager.NotebooksDir;
                var customscreensDir = _pathManager.CustomScreensDir;

                if (_.Item.Direction == SyncDirection.ToDevice)
                {
                    switch (_.Item.Type)
                    {
                        case SyncType.Template:
                            if (_.Item.Action == SyncAction.Add)
                            {
                                _device.Add((Template)_.Item.Data);
                            }
                            else if (_.Item.Action == SyncAction.Remove)
                            {
                                _device.Remove((Template)_.Item.Data);
                            }
                            break;

                        case SyncType.TemplateConfig:
                            var templatesDir = _pathManager.TemplatesDir;

                            scp.Upload(new FileInfo(Path.Combine(templatesDir, "templates.json")), PathList.Templates + "/templates.json");
                            break;

                        case SyncType.Notebook:
                            if (_.Item.Action == SyncAction.Remove)
                            {
                                _device.Remove((Metadata)_.Item.Data);
                            }
                            else if (_.Item.Action == SyncAction.Update)
                            {
                                if (_.Item.Data is Metadata md)
                                {
                                    NotificationService.Show("Updating " + md.VisibleName);

                                    scp.Upload(new FileInfo(Path.Combine(notebooksDir, md.ID + ".metadata")), PathList.Documents + "/" + md.ID + ".metadata");
                                }
                            }
                            else if (_.Item.Action == SyncAction.Add)
                            {
                                if (_.Item.Data is Metadata md)
                                {
                                    NotificationService.Show("Uploading " + md.VisibleName);

                                    scp.Upload(new FileInfo(Path.Combine(notebooksDir, md.ID + ".metadata")), PathList.Documents + "/" + md.ID + ".metadata");

                                    if (md.Type == "DocumentType" && (md.Content.FileType == "pdf" || md.Content.FileType == "epub"))
                                    {
                                        scp.Upload(new FileInfo(Path.Combine(notebooksDir, md.ID + "." + md.Content.FileType)), PathList.Documents + "/" + md.ID + "." + md.Content.FileType);
                                        scp.Upload(new FileInfo(Path.Combine(notebooksDir, md.ID + ".content")), PathList.Documents + "/" + md.ID + ".content");
                                    }
                                }
                            }

                            break;

                        case SyncType.Screen:
                            if (_.Item.Data is CustomScreen cs && _.Item.Action == SyncAction.Add)
                            {
                                NotificationService.Show("Uploading Screen" + cs.Title);

                                scp.Upload(new FileInfo(Path.Combine(customscreensDir, cs.Filename)), PathList.Screens + cs.Filename);
                            }
                            break;
                    }
                }
                else
                {
                    if (_.Item.Action == SyncAction.Remove)
                    {
                        var data = (Metadata)_.Item.Data;
                        _local.Remove(data);
                        MetadataStorage.Local.Remove(data);
                        _syncService.NotebooksFilter.Documents.Clear();

                        foreach (var mds in MetadataStorage.Local.GetByParent(_syncService.NotebooksFilter.Folder))
                        {
                            _syncService.NotebooksFilter.Documents.Add(mds);
                        }
                    }
                }
            });

            MessageRouter.Register<InitStorageMessage>(_ =>
            {
                _device.GetTemplates();

                _syncService.LoadFromLocal();

                _device.DownloadCustomScreens();
            });

            MessageRouter.Register<CheckForUpdateMessage>(async _ =>
            {
                NotificationService.Show("Checking for Updates");

                await Updater.StartUpdate();
            });

            MessageRouter.Register<AttentionRequiredMessage>(async _ =>
            {
                var result = await DialogService.ShowDialog(_.Text);

                if (result)
                {
                    _.Action(_);
                }
            });

            MessageRouter.Register<PostActionMessage>(_ =>
            {
                _.Action();
            });

            MessageRouter.Register<DownloadNotebooksMessage>(_ =>
            {
                var notebooksDir = _pathManager.NotebooksDir;

                NotificationService.Show("Downloading Notebook Metadata");

                var cmd = client.RunCommand("ls -p " + PathList.Documents);
                var allFilenames = cmd.Result.Split('\n', StringSplitOptions.RemoveEmptyEntries).Where(_ => !_.EndsWith(".zip") && !_.EndsWith(".zip.part"));
                var mds = new List<Metadata>();
                var mdFilenames = allFilenames.Where(_ => _.EndsWith(".metadata")).ToArray();
                var mdLocals = new Dictionary<string, Metadata>();

                for (var i = 0; i < mdFilenames.Length; i++)
                {
                    var md = mdFilenames[i];
                    NotificationService.Show($"Downloading Notebook Metadata {i} / {mdFilenames.Length}");

                    var mdContent = client.RunCommand("cat " + PathList.Documents + "/" + md).Result;
                    var contentContent = "{}";
                    var pageDataContent = "";

                    if (allFilenames.Contains(Path.ChangeExtension(md, ".content")))
                    {
                        contentContent = client.RunCommand("cat " + PathList.Documents + "/" + Path.ChangeExtension(md, ".content")).Result;
                    }
                    if (allFilenames.Contains(Path.ChangeExtension(md, ".pagedata")))
                    {
                        pageDataContent = client.RunCommand("cat " + PathList.Documents + "/" + Path.ChangeExtension(md, ".pagedata")).Result;
                    }

                    if (string.IsNullOrEmpty(mdContent))
                    {
                        continue;
                    }

                    var mdObj = JsonConvert.DeserializeObject<Metadata>(mdContent);
                    var contentObj = JsonConvert.DeserializeObject<ContentFile>(contentContent);
                    Metadata mdLocalObj;

                    try
                    {
                        mdLocalObj = JsonConvert.DeserializeObject<Metadata>(File.ReadAllText(Path.Combine(notebooksDir, md)));
                    }
                    catch
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
                            _syncService.NotebooksFilter.Documents.Add(mdObj);
                        }
                    }
                }

                var notebook = 0;
                foreach (var md in mds)
                {
                    if (md.Content.FileType == "notebook")
                    {
                        var allFolders = allFilenames.Where(_ => _.StartsWith(md.ID) && _.EndsWith("/"));

                        foreach (var folder in allFolders)
                        {
                            NotificationService.Show($"Downloading Folder {folder} {notebook}/{allFilenames.Where(_ => _.EndsWith("/")).Count()}");

                            var directoryInfo = new DirectoryInfo(Path.Combine(notebooksDir, folder));
                            if (!directoryInfo.Exists)
                            {
                                directoryInfo.Create();
                            }

                            scp.Download(PathList.Documents + "/" + folder, directoryInfo);
                        }
                    }
                    else
                    {
                        var otherfiles = allFilenames.Where(_ => !_.EndsWith(".metadata") && !_.EndsWith("/") && _.StartsWith(md.ID)).ToArray();

                        scp.Downloading += OnDownloading;

                        for (var i = 0; i < otherfiles.Length; i++)
                        {
                            var fi = new FileInfo(Path.Combine(notebooksDir, otherfiles[i]));
                            if (!md.Deleted)
                            {
                                if (md.Version > mdLocals[md.ID].Version)
                                {
                                    if (!fi.Exists)
                                    {
                                        scp.Download(PathList.Documents + "/" + otherfiles[i], fi);
                                    }
                                }
                            }
                        }

                        scp.Downloading -= OnDownloading;
                    }

                    MetadataStorage.Local.Add(md, out var alreadyAdded);

                    if (md.Parent == "" && !alreadyAdded)
                    {
                        _syncService.NotebooksFilter.Documents.Add(md);
                    }
                    notebook++;
                }

                NotificationService.Hide();
            });

            MessageRouter.Register<HideStatusMessage>(_ =>
            {
                NotificationService.Hide();
            });

            MessageRouter.Register<ShowStatusMessage>(_ =>
            {
                NotificationService.Show(_.Message);
            });
        }

        public void Post(AsynchronousMessage msg)
        {
            _mailbox.Post(msg);
        }

        public void PostAction(Action p)
        {
            _mailbox.Post(new PostActionMessage(p));
        }

        private void OnDownloading(object sender, ScpDownloadEventArgs e)
        {
            NotificationService.Show($"Downloading {e.Filename} {e.Downloaded:n0} Bytes/ {e.Size:n0} Bytes");
        }
    }
}
