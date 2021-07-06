using System.IO;
using System.Linq;
using LiteDB;
using Newtonsoft.Json;
using Slithin.Core.Remarkable;

namespace Slithin.Core.Sync.Repositorys
{
    public class LocalRepository : IRepository
    {
        public LocalRepository()
        {
        }

        public void Add(Template template)
        {
        }

        public Template[] GetTemplates()
        {
            var path = Path.Combine(ServiceLocator.ConfigBaseDir, "templates.json");
            return JsonConvert.DeserializeObject<TemplateStorage>(File.ReadAllText(path)).Templates;
        }
    }
}
