using Newtonsoft.Json;

namespace Slithin.Core.Remarkable
{
    public class TemplateStorage
    {

        [JsonProperty("templates")]
        public Template[]? Templates { get; set; }

        public static TemplateStorage? Instance = new();

        public void Load()
        {
            var content = ServiceLocator.Client.RunCommand("cat " + PathList.Templates + "/templates.json").Result;
            var templates = JsonConvert.DeserializeObject<TemplateStorage>(content);

            Instance = templates;

            foreach (var item in templates.Templates)
            {
                item.Load();
            }
        }

        public void Apply()
        {
            var result = ServiceLocator.Client.RunCommand("systemctl restart xochitl");

            if (result.ExitStatus != 0)
            {
                System.Console.WriteLine(result.Error);
            }
        }
    }
}
