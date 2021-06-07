using System.Text.Json.Serialization;

namespace Slithin.Core.Remarkable
{
    public class TemplateStorage
    {

        [JsonPropertyName("templates")]
        public Template[]? Templates { get; set; }

        public static TemplateStorage? Instance = new();

        public void Load()
        {
            var content = ServiceLocator.Client.RunCommand("cat " + PathList.Templates + "/templates.json").Result;
            var templates = System.Text.Json.JsonSerializer.Deserialize<TemplateStorage>(content);

            Instance = templates;
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
