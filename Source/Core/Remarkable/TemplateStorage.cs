using System.IO;
using Newtonsoft.Json;

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
            var content = ServiceLocator.Client.RunCommand("cat " + PathList.Templates + "/templates.json").Result;
            var templates = JsonConvert.DeserializeObject<TemplateStorage>(content);

            Instance = templates;

            foreach (var item in templates.Templates)
            {
                item.OnDevice = true;
                item.Load();
            }

            File.WriteAllText("templates.json", content);
        }
    }
}
