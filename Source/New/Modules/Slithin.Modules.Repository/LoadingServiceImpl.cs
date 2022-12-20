using AuroraModularis.Core;
using Slithin.Entities.Remarkable;
using Slithin.Modules.Diagnostics.Sentry.Models;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.Repository.Models;
using Slithin.Modules.Sync.Models;

namespace Slithin.Modules.Repository;

internal class LoadingServiceImpl : ILoadingService
{
    private Container _container;

    public LoadingServiceImpl(Container container)
    {
        _container = container;
    }

    public void LoadApiToken()
    {
        //var settings = _settingsService.GetSettings();

        /*
        if (settings.MarketplaceCredential != null)
        {
            var authThread = new Thread(() =>
            {
                var api = new MarketplaceAPI();
                api.Authenticate(settings.MarketplaceCredential.Username, settings.MarketplaceCredential.HashedPassword);

                ServiceLocator.Container.Register(api);
            });
            authThread.Start();
        }
        */
    }

    public void LoadNotebooks()
    {
        var errorTrackingService = _container.Resolve<IDiagnosticService>();
        var mdStorage = _container.Resolve<IMetadataRepository>();
        var pathManager = _container.Resolve<IPathManager>();
        var localisationService = _container.Resolve<ILocalisationService>();

        var monitor = errorTrackingService.StartPerformanceMonitoring("Loading", "Notebooks");

        mdStorage.Clear();

        var filter = Container.Current.Resolve<NotebooksFilter>();

        filter.Documents = new();

        foreach (var md in Directory.GetFiles(pathManager.NotebooksDir, "*.metadata", SearchOption.AllDirectories))
        {
            var mdObj = mdStorage.Load(Path.GetFileNameWithoutExtension(md));

            mdStorage.AddMetadata(mdObj, out _);
        }

        filter.Documents.Add(new Metadata
        {
            Type = "CollectionType",
            VisibleName = localisationService.GetString("Trash"),
            ID = "trash"
        });

        foreach (var md in mdStorage.GetByParent(""))
        {
            filter.Documents.Add(md);
        }

        filter.SortByFolder();

        monitor.Dispose();
    }

    public void LoadTemplates()
    {
        /*
        var monitor = _errorTrackingService.StartPerformanceMonitoring("Loading", "Templates");
        // Load local Templates
        TemplateStorage.Instance?.Load();

        // Load Category Names
        var tempCats = TemplateStorage.Instance?.Templates.Select(x => x.Categories);

        foreach (var item in tempCats)
        {
            foreach (var cat in item)
            {
                if (!ServiceLocator.SyncService.TemplateFilter.Categories.Contains(cat))
                {
                    ServiceLocator.SyncService.TemplateFilter.Categories.Add(cat);
                }
            }
        }

        //Load first templates which are shown to make loading "smoother and faster"
        LoadTemplatesByCategory(ServiceLocator.SyncService.TemplateFilter.Categories.First(), true);

        monitor.Dispose();

        Parallel.ForEach(ServiceLocator.SyncService.TemplateFilter.Categories, (category) =>
        {
            LoadTemplatesByCategory(category);
        });
        */
    }

    public void LoadTemplatesByCategory(string category, bool addToView = false)
    {
        /*
        foreach (var t in TemplateStorage.Instance.Templates)
        {
            if (t.Categories.Contains(category))
            {
                if (!ServiceLocator.SyncService.TemplateFilter.Templates.Contains(t) && addToView)
                {
                    ServiceLocator.SyncService.TemplateFilter.Templates.Add(t);
                }

                t.Load();
            }
        }*/
    }

    public void LoadTools()
    {
        //var toolInvoker = ServiceLocator.Container.Resolve<ToolInvoker>();
        //toolInvoker.Init();
    }
}
