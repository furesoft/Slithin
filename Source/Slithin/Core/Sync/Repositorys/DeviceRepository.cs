using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Renci.SshNet;
using Slithin.Core.Remarkable;
using Slithin.Core.Services;
using Slithin.Models;

namespace Slithin.Core.Sync.Repositorys;

public class DeviceRepository : IRepository
{
    private readonly SshClient _client;
    private readonly ILoadingService _loadingService;
    private readonly LocalRepository _localRepository;
    private readonly IPathManager _pathManager;
    private readonly ScpClient _scp;

    public DeviceRepository(IPathManager pathManager, LocalRepository localRepository, ScpClient scp, SshClient client,
        ILoadingService loadingService)
    {
        _pathManager = pathManager;
        _localRepository = localRepository;
        _scp = scp;
        _client = client;
        _loadingService = loadingService;
    }

    public void Add(CustomScreen screen)
    {
        _scp.Upload(new FileInfo(Path.Combine(_pathManager.CustomScreensDir, screen.Title + ".png")),
            PathList.Screens + screen.Title + ".png");
    }

    public void AddTemplate(Template template)
    {
        //1. copy template to device
        //2. add template to template.json

        var deviceTemplatePath = Path.Combine(_pathManager.TemplatesDir, template.Filename + ".png");
        _scp?.Upload(File.OpenRead(deviceTemplatePath), PathList.Templates + template.Filename + ".png");

        // modifiy template.json

        var path = Path.Combine(_pathManager.ConfigBaseDir, "templates.json");
        var templateJson = JsonConvert.DeserializeObject<TemplateStorage>(File.ReadAllText(path));
        var templates = new Template[templateJson!.Templates.Length + 1];
        Array.Copy(templateJson.Templates, templates, templateJson.Templates.Length);

        templates[^1] = template;

        templateJson.Templates = templates;

        var serializerSettings =
            new JsonSerializerSettings { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii };

        File.WriteAllText(path, JsonConvert.SerializeObject(templateJson, serializerSettings));

        // upload modified template.json
        var jsonStrm = File.OpenRead(path);
        _scp!.Upload(jsonStrm, PathList.Templates + "/templates.json");
    }

    public void DownloadCustomScreens()
    {
        var cmd = _client.RunCommand("ls -p " + PathList.Screens);
        var filenames = cmd.Result.Split('\n', StringSplitOptions.RemoveEmptyEntries).Where(_ => _.EndsWith(".png"));

        // download files to custom screen dir
        foreach (var file in filenames)
        {
            _scp.Download(PathList.Screens + file,
                new FileInfo(Path.Combine(_pathManager.CustomScreensDir, Path.GetFileName(file))));
        }

        _loadingService.LoadScreens();
    }

    public Template[] GetTemplates()
    {
        _scp.Download(PathList.Templates + "templates.json",
            new FileInfo(Path.Combine(_pathManager.ConfigBaseDir, "templates.json")));
        //Get template.json
        //sort out all synced templates
        //download all nonsynced templates to localrepository

        NotificationService.Show("Downloading Templates");

        var path = Path.Combine(_pathManager.ConfigBaseDir, "templates.json");
        var templateJson = JsonConvert.DeserializeObject<TemplateStorage>(File.ReadAllText(path));
        var toSyncTemplates = templateJson.Templates.Where(_ => !_localRepository.GetTemplates().Contains(_));

        foreach (var t in toSyncTemplates)
        {
            _scp.Download(PathList.Templates + "/" + t.Filename + ".png",
                new FileInfo(Path.Combine(_pathManager.TemplatesDir, t.Filename + ".png")));
            _scp.Download(PathList.Templates + "/" + t.Filename + ".svg",
                new FileInfo(Path.Combine(_pathManager.TemplatesDir, t.Filename + ".svg")));
        }

        NotificationService.Hide();

        return null;
    }

    public void Remove(Metadata data)
    {
        if (data.Type != "DocumentType")
        {
            return;
        }

        var cmd = _client.RunCommand("ls " + PathList.Documents);
        var split = cmd.Result.Split('\n');
        var excluded = split.Where(_ => _.Contains(data.ID));

        foreach (var filename in excluded.Select(_ => PathList.Documents + _))
        {
            _client.RunCommand("rm -fr " + filename);
        }
    }

    public void RemoveTemplate(Template template)
    {
        // modifiy template.json
        var path = Path.Combine(_pathManager.ConfigBaseDir, "templates.json");
        var templateJson = JsonConvert.DeserializeObject<TemplateStorage>(File.ReadAllText(path));
        templateJson.Remove(template);
        templateJson.Save();

        _scp.Upload(File.OpenRead(path),
            Path.Combine(PathList.Templates, "templates.json"));
        _client.RunCommand("rm -fr " + PathList.Templates + template.Filename + ".png");
    }
}
