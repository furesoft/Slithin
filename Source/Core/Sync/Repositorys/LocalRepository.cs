using System;
using System.IO;
using Newtonsoft.Json;
using Slithin.Core.Remarkable;
using Slithin.Core.Services;

namespace Slithin.Core.Sync.Repositorys
{
    public class LocalRepository : IRepository
    {
        private readonly IPathManager _pathManager;

        public LocalRepository(IPathManager pathManager)
        {
            _pathManager = pathManager;
        }

        public void Add(Template template)
        {
            var path = Path.Combine(_pathManager.ConfigBaseDir, "templates.json");

            if (!File.Exists(path))
            {
                File.WriteAllText(path, "{\"templates\": []}");
            }

            var templateJson = JsonConvert.DeserializeObject<TemplateStorage>(File.ReadAllText(path));
            var templates = new Template[templateJson.Templates.Length + 1];
            Array.Copy(templateJson.Templates, templates, templateJson.Templates.Length);

            templates[^1] = template;

            templateJson.Templates = templates;

            File.WriteAllText(path, JsonConvert.SerializeObject(templateJson, Formatting.Indented));
        }

        public void AddScreen(string filename, string localfilename)
        {
            File.Delete(Path.Combine(_pathManager.CustomScreensDir, localfilename));
            File.Copy(filename, Path.Combine(_pathManager.CustomScreensDir, localfilename));
        }

        public Template[] GetTemplates()
        {
            var path = Path.Combine(_pathManager.ConfigBaseDir, "templates.json");
            return JsonConvert.DeserializeObject<TemplateStorage>(File.ReadAllText(path)).Templates;
        }

        public Version GetVersion()
        {
            if (!File.Exists(Path.Combine(_pathManager.ConfigBaseDir, ".version")))
            {
                File.WriteAllText(Path.Combine(_pathManager.ConfigBaseDir, ".version"), ServiceLocator.Device.GetVersion().ToString());
            }

            return new Version(File.ReadAllText(Path.Combine(_pathManager.ConfigBaseDir, ".version")));
        }

        public void Remove(Metadata md)
        {
            if (md.Type == "DocumentType")
            {
                var files = Directory.GetFiles(_pathManager.NotebooksDir, md.ID + ".*");
                foreach (var file in files)
                {
                    File.Delete(file);
                }

                var di = new DirectoryInfo(Path.Combine(_pathManager.NotebooksDir, md.ID));
                if (di.Exists)
                {
                    di.Delete(true);
                }
            }
            else
            {
                File.Delete(Path.Combine(_pathManager.NotebooksDir, md.ID + ".metadata"));
            }
        }

        public void Remove(Template template)
        {
            File.Delete(Path.Combine(_pathManager.TemplatesDir, template.Filename + ".png"));
        }

        public void UpdateVersion(Version version)
        {
            File.WriteAllText(Path.Combine(_pathManager.ConfigBaseDir, ".version"), version.ToString());
        }
    }
}
