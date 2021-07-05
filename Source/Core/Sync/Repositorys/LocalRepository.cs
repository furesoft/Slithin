using System.IO;
using System.Linq;
using LiteDB;
using Slithin.Core.Remarkable;

namespace Slithin.Core.Sync.Repositorys
{
    public class LocalRepository : IRepository
    {
        private readonly LiteDatabase _database;
        private readonly ILiteCollection<Template> _templates;

        public LocalRepository()
        {
            _database = new(Path.Combine(ServiceLocator.ConfigBaseDir, "slithin.local"));
            _templates = _database.GetCollection<Template>();
        }

        public void Add(Template template)
        {
            _templates.Insert(template);
        }

        public string[] GetTemplateNames()
        {
            return _templates.FindAll().Select(_ => _.Name).ToArray();
        }

        public Template[] GetTemplates()
        {
            return _templates.FindAll().ToArray();
        }
    }
}
