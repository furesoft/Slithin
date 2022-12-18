using AuroraModularis.Core;
using Newtonsoft.Json;
using Slithin.Entities.Remarkable;
using Slithin.Modules.Repository.Models;

namespace Slithin.Modules.Repository;

public class LocalRepository : IRepository
{
    private readonly Container _container;

    public LocalRepository(Container container)
    {
        _container = container;
    }

    public void AddScreen(Stream strm, string localfilename)
    {
        var pathManager = _container.Resolve<IPathManager>();

        File.Delete(Path.Combine(pathManager.CustomScreensDir, localfilename));
        var outputStrm = File.OpenWrite(Path.Combine(pathManager.CustomScreensDir, localfilename));

        strm.CopyTo(outputStrm);
        strm.Dispose();

        outputStrm.Dispose();
    }

    public void AddTemplate(Template template)
    {
        var pathManager = _container.Resolve<IPathManager>();

        var path = Path.Combine(pathManager.ConfigBaseDir, "templates.json");

        if (!File.Exists(path))
        {
            File.WriteAllText(path, "{\"templates\": []}");
        }

        var templateJson = JsonConvert.DeserializeObject<TemplateStorage>(File.ReadAllText(path));
        var templates = new Template[templateJson.Templates.Length + 1];
        Array.Copy(templateJson.Templates, templates, templateJson.Templates.Length);

        templates[^1] = template;

        templateJson.Templates = templates;

        var serializerSettings = new JsonSerializerSettings();
        serializerSettings.StringEscapeHandling = StringEscapeHandling.EscapeNonAscii;

        File.WriteAllText(path, JsonConvert.SerializeObject(templateJson, Formatting.Indented, serializerSettings));
    }

    public Template[] GetTemplates()
    {
        var pathManager = _container.Resolve<IPathManager>();

        var path = Path.Combine(pathManager.ConfigBaseDir, "templates.json");

        return JsonConvert.DeserializeObject<TemplateStorage>(File.ReadAllText(path)).Templates;
    }

    public Version GetVersion()
    {
        var pathManager = _container.Resolve<IPathManager>();
        var versionService = _container.Resolve<IVersionService>();

        if (!File.Exists(Path.Combine(pathManager.ConfigBaseDir, ".version")))
        {
            File.WriteAllText(Path.Combine(pathManager.ConfigBaseDir, ".version"),
                versionService.GetDeviceVersion().ToString());
        }

        return versionService.GetLocalVersion();
    }

    public void Remove(Metadata md)
    {
        var pathManager = _container.Resolve<IPathManager>();

        var files = Directory.GetFiles(pathManager.NotebooksDir, md.ID + "*");

        foreach (var file in files)
        {
            File.Delete(file);
        }

        var directories = Directory.GetDirectories(pathManager.NotebooksDir, md.ID + "*");

        foreach (var directory in directories)
        {
            Directory.Delete(directory, true);
        }
    }

    public void RemoveTemplate(Template template)
    {
        var pathManager = _container.Resolve<IPathManager>();

        //File.Delete(Path.Combine(pathManager.TemplatesDir, template.Filename + ".png"));
    }

    public void UpdateVersion(Version version)
    {
        var pathManager = _container.Resolve<IPathManager>();

        if (!Directory.Exists(pathManager.ConfigBaseDir))
        {
            pathManager.Init();
            pathManager.InitDeviceDirectory();
        }

        File.WriteAllText(Path.Combine(pathManager.ConfigBaseDir, ".version"), version.ToString());
    }
}
