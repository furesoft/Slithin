using System;
using System.Collections.Generic;
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

            ServiceLocator.Scp?.Upload(File.OpenRead(Path.Combine(ServiceLocator.TemplatesDir, template.Filename + ".png")), PathList.Templates);

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
            ServiceLocator.Scp.Download(PathList.Templates + "templates.json", new FileInfo(Path.Combine(ServiceLocator.ConfigBaseDir, "templates.json")));
            //Get template.json
            //sort out all synced templates
            //download all nonsynced templates to localrepository

            NotificationService.Show("Downloading Templates");

            var path = Path.Combine(ServiceLocator.ConfigBaseDir, "templates.json");
            var templateJson = JsonConvert.DeserializeObject<TemplateStorage>(File.ReadAllText(path));
            var toSyncTemplates = templateJson.Templates.Where(_ => !ServiceLocator.Local.GetTemplates().Contains(_));

            foreach (var t in toSyncTemplates)
            {
                ServiceLocator.Scp.Download(PathList.Templates + "/" + t.Filename + ".png", new FileInfo(Path.Combine(ServiceLocator.ConfigBaseDir, "Templates", t.Filename + ".png")));
                ServiceLocator.Scp.Download(PathList.Templates + "/" + t.Filename + ".svg", new FileInfo(Path.Combine(ServiceLocator.ConfigBaseDir, "Templates", t.Filename + ".svg")));
            }

            NotificationService.Hide();

            return null;
        }

        public void Remove(Template template)
        {
            // modifiy template.json
            var path = Path.Combine(ServiceLocator.ConfigBaseDir, "templates.json");
            var templateJson = JsonConvert.DeserializeObject<TemplateStorage>(File.ReadAllText(path));
            templateJson.Remove(template);
            templateJson.Save();

            ServiceLocator.Scp.Upload(File.OpenRead(ServiceLocator.ConfigBaseDir + "templates.json"), Path.Combine(PathList.Templates, "templates.json"));
            ServiceLocator.Client.RunCommand("rm -fr " + PathList.Templates + template.Filename + ".png");
        }

        public void Remove(Metadata data)
        {
            if (data.Type == "DocumentType")
            {
                var cmd = ServiceLocator.Client.RunCommand("ls " + PathList.Documents);
                var split = cmd.Result.Split('\n');
                var excluded = split.Where(_ => _.Contains(data.ID));

                var filenames = string.Join(' ', excluded.Select(_ => PathList.Documents + "/" + _));

                ServiceLocator.Client.RunCommand("rm -fr " + filenames);
            }
        }
    }
}
