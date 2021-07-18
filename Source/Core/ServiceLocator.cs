using System;
using System.IO;
using System.Linq;
using Actress;
using LiteDB;
using Renci.SshNet;
using Slithin.Controls;
using Slithin.Core.Remarkable;
using Slithin.Core.Sync.Repositorys;
using Slithin.Messages;
using Slithin.ViewModels;
using Slithin.Core.Sync;

namespace Slithin.Core
{
    public static class ServiceLocator
    {
        public static SshClient Client;
        public static string ConfigBaseDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Slithin");
        public static LiteDatabase Database = new(Path.Combine(ConfigBaseDir, "slithin.db"));
        public static DeviceRepository Device = new();
        public static LocalRepository Local = new();
        public static MailboxProcessor<AsynchronousMessage> Mailbox;
        public static string NotebooksDir = Path.Combine(ConfigBaseDir, "Notebooks");
        public static ScpClient Scp;
        public static SynchronisationService SyncService = new();
        public static string TemplatesDir = Path.Combine(ConfigBaseDir, "Templates");

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

        public static void InitMessageRouter()
        {
            MessageRouter.Register<SyncMessage>(_ =>
            {
                if (_.Item.Direction == Sync.SyncDirection.ToDevice)
                {
                    switch (_.Item.Type)
                    {
                        case Sync.SyncType.Template:
                            if (_.Item.Action == Sync.SyncAction.Add)
                            {
                                Device.Add((Template)_.Item.Data);
                            }
                            else if (_.Item.Action == Sync.SyncAction.Remove)
                            {
                                Device.Remove((Template)_.Item.Data);
                            }
                            break;

                        case Sync.SyncType.TemplateConfig:
                            Scp.Upload(new FileInfo(Path.Combine(TemplatesDir, "templates.json")), PathList.Templates + "/templates.json");
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
