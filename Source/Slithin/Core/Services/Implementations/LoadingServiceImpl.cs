using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Slithin.API.Lib;
using Slithin.Core.Remarkable;
using Slithin.Core.Remarkable.Models;
using Slithin.Core.Tools;

namespace Slithin.Core.Services.Implementations;

public class LoadingServiceImpl : ILoadingService
{
    private readonly ILocalisationService _localisationService;
    private readonly IPathManager _pathManager;
    private readonly ISettingsService _settingsService;

    public LoadingServiceImpl(IPathManager pathManager,
                              ILocalisationService localisationService,
                              ISettingsService settingsService)
    {
        _pathManager = pathManager;
        _localisationService = localisationService;
        _settingsService = settingsService;
    }

    public void LoadApiToken()
    {
        var settings = _settingsService.GetSettings();

        if (settings.MarketplaceCredential != null)
        {
            var api = new MarketplaceAPI();
            api.Authenticate(settings.MarketplaceCredential.Username, settings.MarketplaceCredential.HashedPassword);

            ServiceLocator.Container.Register(api);
        }
    }

    public void LoadNotebooks()
    {
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
    }

    public void LoadScreens()
    {
        Parallel.ForEach(ServiceLocator.SyncService.CustomScreens, (cs) =>
        {
            cs.Load();
        });
    }

    public void LoadTemplates()
    {
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

        Parallel.ForEach(ServiceLocator.SyncService.TemplateFilter.Categories, (category) =>
        {
            LoadTemplatesByCategory(category);
        });
    }

    public void LoadTemplatesByCategory(string category, bool addToView = false)
    {
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
        }
    }

    public void LoadTools()
    {
        var toolInvoker = ServiceLocator.Container.Resolve<ToolInvoker>();
        toolInvoker.Init();
    }
}
