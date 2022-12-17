using System;
using System.IO;
using LiteDB;
using Slithin.Core.Services;
using Slithin.Core.Sync;

namespace Slithin.Core;

public static class ServiceLocator
{
    public static Container Container;

    public static SynchronisationService SyncService;

    public static void Init()
    {
        Container = Container.Current;
        Container.AutoRegister();

        var errorTracker = Container.Resolve<IErrorTrackingService>();
        errorTracker.Init();

        var featureEnabler = Container.Resolve<Slithin.FeatureToggle>();
        featureEnabler.Init();

        var localizer = Container.Resolve<ILocalisationService>();
        localizer.Init();

        var pathManager = Container.Resolve<IPathManager>();
        pathManager.Init();

        var importProviderFactory = Container.Resolve<IImportProviderFactory>();
        importProviderFactory.Init();

        var exportProviderFactory = Container.Resolve<IExportProviderFactory>();
        exportProviderFactory.Init();

        var database = new LiteDatabase(Path.Combine(pathManager.ConfigBaseDir, "slithin.db"));
        Container.Register(database);

        AppDomain.CurrentDomain.ProcessExit += (s, e) =>
        {
            database.Dispose();
        };
    }
}
