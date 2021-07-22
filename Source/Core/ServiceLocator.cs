using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Actress;
using LiteDB;
using Newtonsoft.Json;
using Renci.SshNet;
using Slithin.Controls;
using Slithin.Core.Remarkable;
using Slithin.Core.Sync;
using Slithin.Core.Sync.Repositorys;
using Slithin.Messages;
using Slithin.ViewModels;

namespace Slithin.Core
{
    public static class ServiceLocator
    {
        public static SshClient Client;
        public static string ConfigBaseDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Slithin");

        public static LiteDatabase Database;

        public static DeviceRepository Device;

        public static LocalRepository Local;

        public static MailboxProcessor<AsynchronousMessage> Mailbox;

        public static string NotebooksDir;

        public static ScpClient Scp;

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

            if (!Directory.Exists(ConfigBaseDir))
            {
                Directory.CreateDirectory(ConfigBaseDir);
                Directory.CreateDirectory(TemplatesDir);
                Directory.CreateDirectory(NotebooksDir);

                File.WriteAllText(Path.Combine(ConfigBaseDir, "templates.json"), "{\"templates\": []}");
            }

            Database = new(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Slithin", "slithin.db"));

            Device = new();
            Local = new();

            SyncService = new();
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
                            break;
                    }
                }
                else
                {
                }
            });

            MessageRouter.Register<AttentionRequiredMessage>(async _ =>
            {
                var result = await DialogService.ShowDialog(_.Text);

                if (result)
                {
                    _.Action(_);
                }
            });

            MessageRouter.Register<DownloadAllNotebooksMessage>(_ =>
            {
                NotificationService.Show("Checking Notebooks");

                //get all metadata files, compare with local metadata files and download only newer version

                var cmd = Client.RunCommand("ls -p " + PathList.Documents);
                var allFilenames = cmd.Result.Split('\n');
                var filenames = allFilenames.Where(_ => _.EndsWith(".metadata"));
                var toDownload = new List<string>();

                foreach (var md in filenames)
                {
                    if (File.Exists(Path.Combine(NotebooksDir, md)))
                    {
                        var mdContent = Client.RunCommand("cat " + PathList.Documents + "/" + md).Result;
                        var mdDeviceObj = JsonConvert.DeserializeObject<Metadata>(mdContent);
                        var mdLocalObj = JsonConvert.DeserializeObject<Metadata>(File.ReadAllText(Path.Combine(NotebooksDir, md)));

                        if (mdLocalObj.Version < mdDeviceObj.Version)
                        {
                            toDownload.Add(md);
                        }
                    }
                    else
                    {
                        toDownload.Add(md);
                    }
                }

                for (int i = 0; i < toDownload.Count; i++)
                {
                    NotificationService.Show($"Downloading Notebook {i} of {toDownload.Count()}");

                    foreach (var filename in allFilenames.Where(_ => _.StartsWith(Path.GetFileNameWithoutExtension(
                        toDownload[i]))).Where(_ => !_.EndsWith("/")))
                    {
                        //download
                        Scp.Download(filename, new FileInfo(Path.Combine(NotebooksDir,
                            Path.GetFileName(filename)))
                        );
                    }

                    //download notebook content
                    Scp.Download(Path.GetFileNameWithoutExtension(toDownload[i]),
                        new DirectoryInfo(Path.Combine(NotebooksDir, Path.GetFileNameWithoutExtension(toDownload[i])))
                    );
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
    }
}
