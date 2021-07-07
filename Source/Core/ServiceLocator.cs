using System;
using System.IO;
using System.Linq;
using Actress;
using LiteDB;
using Renci.SshNet;
using Slithin.Core.Remarkable;
using Slithin.Core.Sync.Repositorys;
using Slithin.Messages;
using Slithin.ViewModels;

namespace Slithin.Core
{
    public static class ServiceLocator
    {
        public static SshClient Client;
        public static string ConfigBaseDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Slithin");
        public static LiteDatabase Database = new("slithin.db");
        public static DeviceRepository Device = new();
        public static LocalRepository Local = new();
        public static MailboxProcessor<AsynchronousMessage> Mailbox;
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
                            ServiceLocator.Device.Add((Template)_.Item.Data);
                            break;

                        case Sync.SyncType.TemplateConfig:
                            ServiceLocator.Scp.Upload(new FileInfo(Path.Combine(ServiceLocator.TemplatesDir, "templates.json")), PathList.Templates + "/templates.json");
                            break;
                    }
                }
                else
                {
                }
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
