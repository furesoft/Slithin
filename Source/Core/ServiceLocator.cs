using System.IO;
using LiteDB;
using Slithin.Core.Remarkable.Cloud;
using Slithin.Core.Services;
using Slithin.Core.Sync;

namespace Slithin.Core
{
    public static class ServiceLocator
    {
        public static TinyIoCContainer Container;

        public static SynchronisationService SyncService;

        public static void Init()
        {
            Container = TinyIoCContainer.Current;
            Container.AutoRegister();

            Container.Register<Api>().AsSingleton();
            Container.Register<Storage>().AsSingleton();

            var pathManager = Container.Resolve<IPathManager>();
            pathManager.Init();

            var importProviderFactory = Container.Resolve<IImportProviderFactory>();
            importProviderFactory.Init();

            Container.Register(new LiteDatabase(Path.Combine(pathManager.ConfigBaseDir, "slithin.db")));
        }
    }
}
