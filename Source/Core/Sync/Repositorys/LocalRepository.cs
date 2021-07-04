using System;
using System.IO;
using LiteDB;
using Slithin.Core.Remarkable;

namespace Slithin.Core.Sync.Repositorys
{
    public class LocalRepository : IRepository
    {
        private LiteDatabase _database;
        private ILiteCollection<Template> _templates;

        public LocalRepository()
        {
            _database = new("slithin.local");
            _templates = _database.GetCollection<Template>();
        }

        public void Add(Template template)
        {
            if (!_templates.Exists(Query.EQ("Filename", template.Filename)))
            {
                var ms = new MemoryStream();
                _database.FileStorage.Upload(Guid.NewGuid().ToString(), template.Filename, ms);

                _templates.Insert(template);
            }
        }

        public Template[] GetTemplates()
        {
            throw new System.NotImplementedException();
        }
    }
}
