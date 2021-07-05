using System.IO;
using System.Linq;
using LiteDB;
using Slithin.Core.Remarkable;

namespace Slithin.Core.Sync.Repositorys
{
    public class DeviceRepository : IRepository
    {
        public void Add(Template template)
        {
            //1. copy template to device
            //2. add template to template.json

            ServiceLocator.Scp.Upload(File.OpenRead(Path.Combine(ServiceLocator.TemplatesDir, template.Filename + ".png")), PathList.Templates);

            var jsonStrm = File.OpenRead(Path.Combine(ServiceLocator.ConfigBaseDir, "templates.json"));
            ServiceLocator.Scp.Upload(jsonStrm, PathList.Templates);
        }

        public Template[] GetTemplates()
        {
            ServiceLocator.Scp.Download(Path.Combine(PathList.Templates, "templates.json"), new FileInfo(Path.Combine(ServiceLocator.ConfigBaseDir, "templates.json")));
            //Get template.json
            //sort out all synced templates
            //download all nonsynced templates to localrepository

            return null;
        }
    }

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

        public Template[] GetTemplates()
        {
            return _templates.FindAll().ToArray();
        }
    }
}
