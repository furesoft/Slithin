using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Actress;
using LiteDB;
using NetSparkleUpdater;
using NetSparkleUpdater.Enums;
using NetSparkleUpdater.Events;
using NetSparkleUpdater.SignatureVerifiers;
using Newtonsoft.Json;
using Renci.SshNet;
using Renci.SshNet.Common;
using Slithin.Controls;
using Slithin.Core.Remarkable;
using Slithin.Core.Scripting;
using Slithin.Core.Sync;
using Slithin.Core.Sync.Repositorys;
using Slithin.Messages;
using Slithin.ViewModels;

namespace Slithin.Core
{
    public static class ServiceLocator
    {
        public static string BackupDir;
        public static SshClient Client;
        public static string ConfigBaseDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Slithin");

        public static string CustomScreensDir;
        public static LiteDatabase Database;

        public static DeviceRepository Device;

        public static EventStorage Events;

        public static LocalRepository Local;

        public static MailboxProcessor<AsynchronousMessage> Mailbox;

        public static string NotebooksDir;

        public static ScpClient Scp;

        public static string ScriptsDir;

        public static SynchronisationService SyncService;

        public static string TemplatesDir;

        public static ConnectionWindowViewModel GetLoginCredentials()
        {
            var collection = Database.GetCollection<ConnectionWindowViewModel>();

            if (collection.Count() == 1)
            {
                return collection.FindAll().First();
            }
            else
            {
                return new();
            }
        }

        public static void Init()
        {
            NotebooksDir = Path.Combine(ConfigBaseDir, "Notebooks");
            TemplatesDir = Path.Combine(ConfigBaseDir, "Templates");
            ScriptsDir = Path.Combine(ConfigBaseDir, "Scripts");
            CustomScreensDir = Path.Combine(ConfigBaseDir, "Screens");
            BackupDir = Path.Combine(ConfigBaseDir, "Backups");

            Events = new();

            if (!Directory.Exists(ConfigBaseDir))
            {
                Directory.CreateDirectory(ConfigBaseDir);
                Directory.CreateDirectory(TemplatesDir);
                Directory.CreateDirectory(NotebooksDir);
                Directory.CreateDirectory(ScriptsDir);
                Directory.CreateDirectory(CustomScreensDir);
                Directory.CreateDirectory(BackupDir);

                File.WriteAllText(Path.Combine(ConfigBaseDir, "templates.json"), "{\"templates\": []}");
            }

            Database = new(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Slithin", "slithin.db"));

            Device = new();
            Local = new();

            SyncService = new();

            Mailbox = MailboxProcessor.Start<AsynchronousMessage>(
                async (_) =>
                {
                    while (true)
                    {
                        var msg = await _.Receive();

                        MessageRouter.Route(msg);
                    }
                }
                );

            ServiceLocator.InitMessageRouter();
        }

        public static void InitMessageRouter()
        {
            MessageRouter.Register<SyncMessage>(_ =>
            {
                if (_.Item.Direction == SyncDirection.ToDevice)
                {
                    switch (_.Item.Type)
                    {
                        case SyncType.Template:
                            if (_.Item.Action == SyncAction.Add)
                            {
                                Device.Add((Template)_.Item.Data);
                            }
                            else if (_.Item.Action == SyncAction.Remove)
                            {
                                Device.Remove((Template)_.Item.Data);
                            }
                            break;

                        case SyncType.TemplateConfig:
                            Scp.Upload(new FileInfo(Path.Combine(TemplatesDir, "templates.json")), PathList.Templates + "/templates.json");
                            break;

                        case SyncType.Notebook:
                            if (_.Item.Action == SyncAction.Remove)
                            {
                                Device.Remove((Metadata)_.Item.Data);
                            }
                            else if (_.Item.Action == SyncAction.Update)
                            {
                                if (_.Item.Data is Metadata md)
                                {
                                    NotificationService.Show("Updating " + md.VisibleName);

                                    Scp.Upload(new FileInfo(Path.Combine(NotebooksDir, md.ID + ".metadata")), PathList.Documents + "/" + md.ID + ".metadata");
                                }
                            }
                            else if (_.Item.Action == SyncAction.Add)
                            {
                                if (_.Item.Data is Metadata md)
                                {
                                    NotificationService.Show("Uploading " + md.VisibleName);

                                    Scp.Upload(new FileInfo(Path.Combine(NotebooksDir, md.ID + ".metadata")), PathList.Documents + "/" + md.ID + ".metadata");

                                    if (md.Type == "DocumentType" && (md.Content.FileType == "pdf" || md.Content.FileType == "epub"))
                                    {
                                        Scp.Upload(new FileInfo(Path.Combine(NotebooksDir, md.ID + "." + md.Content.FileType)), PathList.Documents + "/" + md.ID + "." + md.Content.FileType);
                                        Scp.Upload(new FileInfo(Path.Combine(NotebooksDir, md.ID + ".content")), PathList.Documents + "/" + md.ID + ".content");
                                    }
                                }
                            }

                            break;

                        case SyncType.Screen:
                            if (_.Item.Data is CustomScreen cs && _.Item.Action == SyncAction.Add)
                            {
                                NotificationService.Show("Uploading Screen" + cs.Title);

                                Scp.Upload(new FileInfo(Path.Combine(CustomScreensDir, cs.Filename)), PathList.Screens + cs.Filename);
                            }
                            break;
                    }
                }
                else
                {
                    if (_.Item.Action == SyncAction.Remove)
                    {
                        var data = (Metadata)_.Item.Data;
                        Local.Remove(data);
                        MetadataStorage.Local.Remove(data);
                        SyncService.NotebooksFilter.Documents.Clear();

                        foreach (var mds in MetadataStorage.Local.GetByParent(SyncService.NotebooksFilter.Folder))
                        {
                            SyncService.NotebooksFilter.Documents.Add(mds);
                        }
                    }
                }
            });

            MessageRouter.Register<InitStorageMessage>(_ =>
            {
                var templates = Device.GetTemplates();

                SyncService.LoadFromLocal();

                Device.DownloadCustomScreens();
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
                NotificationService.Show("Downloading Notebook Metadata");

                var cmd = Client.RunCommand("ls -p " + PathList.Documents);
                var allFilenames = cmd.Result.Split('\n', StringSplitOptions.RemoveEmptyEntries).Where(_ => !_.EndsWith(".zip") && !_.EndsWith(".zip.part"));
                var mds = new List<Metadata>();
                var mdFilenames = allFilenames.Where(_ => _.EndsWith(".metadata")).ToArray();
                var mdLocals = new Dictionary<string, Metadata>();

                for (var i = 0; i < mdFilenames.Length; i++)
                {
                    var md = mdFilenames[i];
                    NotificationService.Show($"Downloading Notebook Metadata {i} / {mdFilenames.Length}");

                    var mdContent = Client.RunCommand("cat " + PathList.Documents + "/" + md).Result;
                    var contentContent = "{}";
                    var pageDataContent = "";

                    if (allFilenames.Contains(Path.ChangeExtension(md, ".content")))
                    {
                        contentContent = Client.RunCommand("cat " + PathList.Documents + "/" + Path.ChangeExtension(md, ".content")).Result;
                    }
                    if (allFilenames.Contains(Path.ChangeExtension(md, ".pagedata")))
                    {
                        pageDataContent = Client.RunCommand("cat " + PathList.Documents + "/" + Path.ChangeExtension(md, ".pagedata")).Result;
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
                        mdLocalObj = JsonConvert.DeserializeObject<Metadata>(File.ReadAllText(Path.Combine(NotebooksDir, md)));
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

                    if (File.Exists(Path.Combine(NotebooksDir, md)))
                    {
                        if (!mdObj.Deleted)
                        {
                            if (mdObj.Version > mdLocalObj.Version)
                            {
                                if (mdObj.Type == "DocumentType")
                                {
                                    mds.Add(mdObj);
                                }

                                File.WriteAllText(Path.Combine(NotebooksDir, md), mdContent);

                                if (allFilenames.Contains(Path.ChangeExtension(md, ".content")))
                                {
                                    File.WriteAllText(Path.Combine(NotebooksDir, Path.ChangeExtension(md, ".content")), contentContent);
                                }
                                if (allFilenames.Contains(Path.ChangeExtension(md, ".pageData")))
                                {
                                    File.WriteAllText(Path.Combine(NotebooksDir, Path.ChangeExtension(md, ".pageData")), pageDataContent);
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

                        File.WriteAllText(Path.Combine(NotebooksDir, md), mdContent);

                        if (allFilenames.Contains(Path.ChangeExtension(md, ".content")))
                        {
                            File.WriteAllText(Path.Combine(NotebooksDir, Path.ChangeExtension(md, ".content")), contentContent);
                        }
                        if (allFilenames.Contains(Path.ChangeExtension(md, ".pageData")))
                        {
                            File.WriteAllText(Path.Combine(NotebooksDir, Path.ChangeExtension(md, ".pageData")), pageDataContent);
                        }
                    }

                    if (mdObj.Type == "CollectionType" && mdObj.Parent == "")
                    {
                        MetadataStorage.Local.Add(mdObj, out var alreadyAdded);

                        if (!alreadyAdded)
                        {
                            SyncService.NotebooksFilter.Documents.Add(mdObj);
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

                            var directoryInfo = new DirectoryInfo(Path.Combine(NotebooksDir, folder));
                            if (!directoryInfo.Exists)
                            {
                                directoryInfo.Create();
                            }

                            Scp.Download(PathList.Documents + "/" + folder, directoryInfo);
                        }
                    }
                    else
                    {
                        var otherfiles = allFilenames.Where(_ => !_.EndsWith(".metadata") && !_.EndsWith("/") && _.StartsWith(md.ID)).ToArray();

                        Scp.Downloading += OnDownloading;

                        for (var i = 0; i < otherfiles.Length; i++)
                        {
                            var fi = new FileInfo(Path.Combine(NotebooksDir, otherfiles[i]));
                            if (!md.Deleted)
                            {
                                if (md.Version > mdLocals[md.ID].Version)
                                {
                                    if (!fi.Exists)
                                    {
                                        Scp.Download(PathList.Documents + "/" + otherfiles[i], fi);
                                    }
                                }
                            }
                        }

                        Scp.Downloading -= OnDownloading;
                    }

                    MetadataStorage.Local.Add(md, out var alreadyAdded);

                    if (md.Parent == "" && !alreadyAdded)
                    {
                        SyncService.NotebooksFilter.Documents.Add(md);
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

        public static void PostAction(Action p)
        {
            Mailbox.Post(new PostActionMessage(p));
        }

        public static void RememberLoginCredencials(ConnectionWindowViewModel viewModel)
        {
            var collection = Database.GetCollection<ConnectionWindowViewModel>();

            if (collection.Count() == 1)
            {
                //collection.Update(viewModel);
            }
            else
            {
                collection.Insert(viewModel);
            }
        }

        private static void OnDownloading(object sender, ScpDownloadEventArgs e)
        {
            NotificationService.Show($"Downloading {e.Filename} {e.Downloaded:n0} Bytes/ {e.Size:n0} Bytes");
        }
    }
}
