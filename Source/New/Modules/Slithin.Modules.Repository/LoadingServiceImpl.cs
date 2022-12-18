using Slithin.Modules.Diagnostics.Sentry.Models;

using Slithin.Modules.I18N.Models;
using Slithin.Modules.Repository.Models;
using Slithin.Modules.Settings.Models;

namespace Slithin.Modules.Repository;

public class LoadingServiceImpl : ILoadingService
{
    private readonly IDiagnosticService _errorTrackingService;
    private readonly ILocalisationService _localisationService;
    private readonly IPathManager _pathManager;
    private readonly ISettingsService _settingsService;

    public LoadingServiceImpl(IPathManager pathManager,
                              ILocalisationService localisationService,
                              ISettingsService settingsService,
                              IDiagnosticService errorTrackingService)
    {
        _pathManager = pathManager;
        _localisationService = localisationService;
        _settingsService = settingsService;
        _errorTrackingService = errorTrackingService;
    }

    public void LoadApiToken()
    {
        var settings = _settingsService.GetSettings();

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
        /*
        var monitor = _errorTrackingService.StartPerformanceMonitoring("Loading", "Notebooks");

        MetadataStorage.Local.Clear();
        ServiceLocator.SyncService.NotebooksFilter.Documents = new();

        foreach (var md in Directory.GetFiles(_pathManager.NotebooksDir, "*.metadata", SearchOption.AllDirectories))
        {
            var mdObj = Metadata.Load(Path.GetFileNameWithoutExtension(md));

            MetadataStorage.Local.AddMetadata(mdObj, out _);
        }

        ServiceLocator.SyncService.NotebooksFilter.Documents.Add(new Metadata
        {
            Type = "CollectionType",
            VisibleName = _localisationService.GetString("Trash"),
            ID = "trash"
        });

        foreach (var md in MetadataStorage.Local.GetByParent(""))
        {
            ServiceLocator.SyncService.NotebooksFilter.Documents.Add(md);
        }

        ServiceLocator.SyncService.NotebooksFilter.SortByFolder();

        monitor.Dispose();
        */
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
