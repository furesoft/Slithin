using AuroraModularis.Core;
using Slithin.Modules.Diagnostics.Sentry.Models;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.Notebooks.UI.Models;
using Slithin.Modules.Repository.Models;
using Slithin.Modules.Resources.UI;
using Slithin.Modules.Settings.Models;
using Slithin.Modules.Sync.Models;

namespace Slithin.Modules.Repository;

internal class LoadingServiceImpl : ILoadingService
{
    private Container _container;

    public LoadingServiceImpl(Container container)
    {
        _container = container;
    }

    public Task LoadNotebooksAsync()
    {
        var filter = Container.Current.Resolve<NotebooksFilter>();
        var errorTrackingService = _container.Resolve<IDiagnosticService>();
        var mdStorage = _container.Resolve<IMetadataRepository>();
        var pathManager = _container.Resolve<IPathManager>();

        if (filter.Items.Any())
        {
            return Task.CompletedTask;
        }

        var monitor = errorTrackingService.StartPerformanceMonitoring("Loading", "Notebooks");

        mdStorage.Clear();

        filter.Items = new() {new TrashModel()};

        LoadMetadataFiles(pathManager, mdStorage);

        AddMetadatasToFilterWithCorrectParent(mdStorage, filter);

        filter.SortByFolder();

        monitor.Dispose();
        
        return Task.CompletedTask;
    }

    private static void AddMetadatasToFilterWithCorrectParent(IMetadataRepository mdStorage, NotebooksFilter filter)
    {
        foreach (var mds in mdStorage.GetByParent(""))
        {
            if (mds.Type == "CollectionType")
            {
                filter.Items.Add(new DirectoryModel(mds.VisibleName, mds, mds.IsPinned) {ID = mds.ID, Parent = mds.Parent});
            }
            else
            {
                filter.Items.Add(new FileModel(mds.VisibleName, mds, mds.IsPinned) {ID = mds.ID, Parent = mds.Parent});
            }
        }
    }

    private static void LoadMetadataFiles(IPathManager pathManager, IMetadataRepository mdStorage)
    {
        foreach (var md in Directory.GetFiles(pathManager.NotebooksDir, "*.metadata", SearchOption.AllDirectories))
        {
            var mdObj = mdStorage.Load(Path.GetFileNameWithoutExtension(md));

            mdStorage.AddMetadata(mdObj, out _);
        }
    }

    public Task LoadTemplatesAsync()
    {
        var errorTrackingService = Container.Current.Resolve<IDiagnosticService>();
        var storage = Container.Current.Resolve<ITemplateStorage>();
        var filter = Container.Current.Resolve<TemplatesFilter>();

        if (filter.Items.Any())
        {
            return Task.CompletedTask;
        }

        using var monitor = errorTrackingService.StartPerformanceMonitoring("Loading", "Templates");
        // Load local Templates
        storage.Load();

        // Load Category Names
        var tempCats = storage.Templates.Select(x => x.Categories).ToArray();

        foreach (var item in tempCats)
        {
            foreach (var cat in item)
            {
                if (!filter.Categories.Contains(cat))
                {
                    filter.Categories.Add(cat);
                }
            }
        }

        //Load first templates which are shown to make loading "smoother and faster"
        LoadTemplatesByCategory(filter.Categories.First(), filter, storage, true);

        Parallel.ForEach(filter.Categories, (category) =>
        {
            LoadTemplatesByCategory(category, filter, storage);
        });
        return Task.CompletedTask;
    }

    private async Task LoadTemplatesByCategory(string category, TemplatesFilter filter, ITemplateStorage storage, bool addToView = false)
    {
        foreach (var t in storage.Templates)
        {
            if (!t.Categories.Contains(category))
            {
                continue;
            }

            if (!filter.Items.Contains(t) && addToView)
            {
                filter.Items.Add(t);
            }

            await storage.LoadTemplateAsync(t);
        }
    }
}
