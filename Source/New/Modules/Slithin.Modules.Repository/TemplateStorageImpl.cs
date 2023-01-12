using AuroraModularis.Core;
using Avalonia.Media.Imaging;
using Newtonsoft.Json;
using Slithin.Entities.Remarkable;
using Slithin.Modules.Repository.Models;

namespace Slithin.Modules.Repository;

public class TemplateStorageImpl : ITemplateStorage
{
    [JsonProperty("templates")] public Template[] Templates { get; set; }

    public void AppendTemplate(Template template)
    {
        var tmp = new Template[Templates.Length + 1];
        Array.Copy(Templates, tmp, Templates.Length);

        tmp[^1] = template;

        Templates = tmp;
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
        var pathManager = Container.Current.Resolve<IPathManager>();

        var serializerSettings = new JsonSerializerSettings();
        serializerSettings.StringEscapeHandling = StringEscapeHandling.EscapeNonAscii;

        var path = Path.Combine(pathManager.ConfigBaseDir, "templates.json");
        File.WriteAllText(path, JsonConvert.SerializeObject(this, Formatting.Indented, serializerSettings));
    }

    public async Task LoadTemplateAsync(Template template)
    {
        var templatesDir = Container.Current.Resolve<IPathManager>().TemplatesDir;

        if (!Directory.Exists(templatesDir) || template.Image is not null)
        {
            return;
        }

        var path = Path.Combine(templatesDir, template.Filename);

        var filename = $"{path}.png";
        if (!File.Exists(filename))
        {
            return;
        }

        template.Image = Bitmap.DecodeToWidth(File.OpenRead(filename), 150);
    }

    public void Load()
    {
        var pathManager = Container.Current.Resolve<IPathManager>();

        var serializerSettings = new JsonSerializerSettings();
        serializerSettings.StringEscapeHandling = StringEscapeHandling.EscapeNonAscii;

        var path = Path.Combine(pathManager.ConfigBaseDir, "templates.json");
        var json = File.ReadAllText(path);

        Templates = JsonConvert.DeserializeObject<TemplateStorageImpl>(json, serializerSettings).Templates;
    }
}
