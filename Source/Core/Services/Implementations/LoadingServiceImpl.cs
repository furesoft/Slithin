using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Slithin.Core.Remarkable;

namespace Slithin.Core.Services.Implementations
{
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

            foreach (var md in Directory.GetFiles(_pathManager.NotebooksDir, "*.metadata", SearchOption.AllDirectories))
            {
                var mdObj = JsonConvert.DeserializeObject<Metadata>(File.ReadAllText(md));
                mdObj.ID = Path.GetFileNameWithoutExtension(md);

                if (File.Exists(Path.ChangeExtension(md, ".content")))
                {
                    mdObj.Content = JsonConvert.DeserializeObject<ContentFile>(File.ReadAllText(Path.ChangeExtension(md, ".content")));
                }
                if (File.Exists(Path.ChangeExtension(md, ".pagedata")))
                {
                    mdObj.PageData.Parse(File.ReadAllText(Path.ChangeExtension(md, ".pagedata")));
                }

                MetadataStorage.Local.Add(mdObj, out var alreadyAdded);
            }

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
            // Load local Templates
            TemplateStorage.Instance?.Load();

            // Load Category Names
            var tempCats = TemplateStorage.Instance?.Templates.Select(_ => _.Categories);
            ServiceLocator.SyncService.TemplateFilter.Categories.Add("All");

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
        }
    }
}
