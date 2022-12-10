using Newtonsoft.Json;

namespace Slithin.Entities;

public class TemplateStorage
{
    public static TemplateStorage Instance = new();

    [JsonProperty("templates")] public Template[] Templates { get; set; }

    public void AppendTemplate(Template template)
    {
        var tmp = new Template[Templates.Length + 1];
        Array.Copy(Templates, tmp, Templates.Length);

        tmp[^1] = template;

        Templates = tmp;
    }

    public void Load()
    {
        Instance.Templates = ServiceLocator.Container.Resolve<LocalRepository>().GetTemplates();
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
        var pathManager = ServiceLocator.Container.Resolve<IPathManager>();

        var serializerSettings = new JsonSerializerSettings();
        serializerSettings.StringEscapeHandling = StringEscapeHandling.EscapeNonAscii;

        var path = Path.Combine(pathManager.ConfigBaseDir, "templates.json");
        File.WriteAllText(path, JsonConvert.SerializeObject(this, Formatting.Indented, serializerSettings));
    }
}
