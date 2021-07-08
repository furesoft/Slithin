using System;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

namespace Slithin.Core.Remarkable
{
    //ToDo save all templates and templates.json into liteb file
    public class TemplateStorage
    {
        public static TemplateStorage? Instance = new();

        [JsonProperty("templates")]
        public Template[]? Templates { get; set; }

        public void Apply()
        {
            var result = ServiceLocator.Client.RunCommand("systemctl restart xochitl");

            if (result.ExitStatus != 0)
            {
                System.Console.WriteLine(result.Error);
            }
        }

        public void Load()
        {
            Instance.Templates = ServiceLocator.Local.GetTemplates();

            foreach (var item in Instance.Templates)
            {
                item.OnDevice = true;
                item.Load();
            }
        }

        public void Remove(Template tmpl)
        {
            var tmp = Templates.ToList();
            tmp.Remove(tmpl);

            Templates = tmp.ToArray();

            Save();
        }

        public void Save()
        {
            var path = Path.Combine(ServiceLocator.ConfigBaseDir, "templates.json");
            File.WriteAllText(path, JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }
}
