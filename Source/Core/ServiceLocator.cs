using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Actress;
using LiteDB;
using Newtonsoft.Json;
using Renci.SshNet;
using Renci.SshNet.Common;
using Slithin.Controls;
using Slithin.Core.Remarkable;
using Slithin.Core.Scripting;
using Slithin.Core.Services;
using Slithin.Core.Services.Implementations;
using Slithin.Core.Sync;
using Slithin.Core.Sync.Repositorys;
using Slithin.Messages;
using Slithin.ViewModels;

namespace Slithin.Core
{
    public static class ServiceLocator
    {
        public static SshClient Client;

        public static TinyIoCContainer Container;
        public static DeviceRepository Device;

        public static LocalRepository Local;

        public static ScpClient Scp;

        public static SynchronisationService SyncService;

        public static void Init()
        {
            Container = TinyIoCContainer.Current;
            Container.AutoRegister();

            Container.Register<Automation>().AsSingleton();

            var pathManager = Container.Resolve<IPathManager>();
            pathManager.Init();

            Container.Register(new LiteDatabase(Path.Combine(pathManager.ConfigBaseDir, "slithin.db")));
            SyncService = new(pathManager, Container.Resolve<LiteDatabase>());

            Container.Resolve<IMailboxService>().Init();

            Device = Container.Resolve<DeviceRepository>();
            Local = Container.Resolve<LocalRepository>();
        }
    }
}
