﻿using System;
using System.IO;
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
            ServiceLocator.Scp.Download(PathList.Templates + "templates.json", new FileInfo(Path.Combine(ServiceLocator.ConfigBaseDir, "templates.json")));
            //Get template.json
            //sort out all synced templates
            //download all nonsynced templates to localrepository

            return null;
        }
    }
}
