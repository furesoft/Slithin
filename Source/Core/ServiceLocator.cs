using System.IO;
using LiteDB;
using Renci.SshNet;
using Slithin.Core.Remarkable.Cloud;
using Slithin.Core.Scripting;
using Slithin.Core.Services;
using Slithin.Core.Sync;
using Slithin.Core.Sync.Repositorys;

namespace Slithin.Core
{
    public static class ServiceLocator
    {
        public static SshClient Client;

        public static TinyIoCContainer Container;

        public static ScpClient Scp;

        public static SynchronisationService SyncService;

        public static void Init()
        {
            Container = TinyIoCContainer.Current;
            Container.AutoRegister();

            Container.Register<Automation>().AsSingleton();
            Container.Register<Api>().AsSingleton();
            Container.Register<Storage>().AsSingleton();

            var pathManager = Container.Resolve<IPathManager>();
            pathManager.Init();

            Container.Register(new LiteDatabase(Path.Combine(pathManager.ConfigBaseDir, "slithin.db")));
            SyncService = new(pathManager, Container.Resolve<LiteDatabase>(), Container.Resolve<LocalRepository>());

            Container.Resolve<IMailboxService>().Init();
        }
    }
}
