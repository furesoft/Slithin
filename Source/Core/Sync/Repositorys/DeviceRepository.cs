using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
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

            // modifiy template.json
            var path = Path.Combine(ServiceLocator.ConfigBaseDir, "templates.json");
            var templateJson = JsonConvert.DeserializeObject<TemplateStorage>(File.ReadAllText(path));
            Template[] templates = new Template[templateJson.Templates.Length + 1];
            Array.Copy(templateJson.Templates, templates, templateJson.Templates.Length);

            templates[^1] = template;

            templateJson.Templates = templates;

            File.WriteAllText(path, JsonConvert.SerializeObject(templateJson));

            // upload modified template.json
            var jsonStrm = File.OpenRead(path);
            ServiceLocator.Scp.Upload(jsonStrm, PathList.Templates + "/templates.json");
        }

        public Template[] GetTemplates()
        {
            if (!Directory.Exists(ServiceLocator.TemplatesDir))
            {
                Directory.CreateDirectory(ServiceLocator.TemplatesDir);
            }

            ServiceLocator.Scp.Download(PathList.Templates + "templates.json", new FileInfo(Path.Combine(ServiceLocator.ConfigBaseDir, "templates.json")));
            //Get template.json
            //sort out all synced templates
            //download all nonsynced templates to localrepository

            var path = Path.Combine(ServiceLocator.ConfigBaseDir, "templates.json");
            var templateJson = JsonConvert.DeserializeObject<TemplateStorage>(File.ReadAllText(path));
            var toSyncTemplates = templateJson.Templates.Where(_ => !ServiceLocator.Local.GetTemplates().Contains(_));

            foreach (var t in toSyncTemplates)
            {
                ServiceLocator.Scp.Download(PathList.Templates + "/" + t.Filename + ".png", new FileInfo(Path.Combine(ServiceLocator.ConfigBaseDir, "Templates", t.Filename + ".png")));
            }

            return null;
        }

        public void Remove(Template template)
        {
            throw new NotImplementedException();
        }
    }
}
