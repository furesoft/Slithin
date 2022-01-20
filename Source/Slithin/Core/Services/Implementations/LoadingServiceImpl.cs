using System.IO;
using System.Linq;
using Slithin.Core.Remarkable;

namespace Slithin.Core.Services.Implementations;

public class LoadingServiceImpl : ILoadingService
{
    private readonly IPathManager _pathManager;

    public LoadingServiceImpl(IPathManager pathManager)
    {
        _pathManager = pathManager;
    }

    public void LoadNotebooks()
    {
        MetadataStorage.Local.Clear();
        ServiceLocator.SyncService.NotebooksFilter.Documents.Clear();

        foreach (var md in Directory.GetFiles(_pathManager.NotebooksDir, "*.metadata", SearchOption.AllDirectories))
        {
            var mdObj = Metadata.Load(Path.GetFileNameWithoutExtension(md));

            MetadataStorage.Local.AddMetadata(mdObj, out _);
        }

        ServiceLocator.SyncService.NotebooksFilter.Documents.Add(new Metadata
        {
            Type = "CollectionType",
            VisibleName = "Trash",
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
        foreach (var cs in ServiceLocator.SyncService.CustomScreens)
        {
            cs.Load();
        }
    }

    public void LoadTemplates()
    {
        ServiceLocator.SyncService.TemplateFilter.Templates.Clear();

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

        //Load first templates which are shown to make loading "faster"
        LoadTemplatesByCategory(ServiceLocator.SyncService.TemplateFilter.Categories.First());

        foreach (var category in ServiceLocator.SyncService.TemplateFilter.Categories)
        {
            LoadTemplatesByCategory(category);
        }
    }

    public void LoadTemplatesByCategory(string category)
    {
        foreach (var t in TemplateStorage.Instance.Templates)
        {
            if (!ServiceLocator.SyncService.TemplateFilter.Templates.Contains(t))
            {
                ServiceLocator.SyncService.TemplateFilter.Templates.Add(t);
            }
            
            ServiceLocator.SyncService.TemplateFilter.Templates.Add(t);

            if (t.Categories.Contains(category))
            {
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
