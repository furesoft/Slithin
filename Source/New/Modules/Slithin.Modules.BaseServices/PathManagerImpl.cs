﻿using Slithin.Modules.BaseServices.Models;

namespace Slithin.Modules.BaseServices;

internal class PathManagerImpl : IPathManager
{
    public string BackupsDir => Path.Combine(ConfigBaseDir, "Backups");
    public string ConfigBaseDir { get; set; }

    public string CustomScreensDir => Path.Combine(ConfigBaseDir, "Screens");

    public string DevicesDir => Path.Combine(SlithinDir, "Devices");
    public string NotebooksDir => Path.Combine(ConfigBaseDir, "Notebooks");
    public string SlithinDir => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Slithin");
    public string TemplatesDir => Path.Combine(ConfigBaseDir, "Templates");

    public void Init()
    {
        ConfigBaseDir = SlithinDir;

        if (!Directory.Exists(SlithinDir))
        {
            Directory.CreateDirectory(SlithinDir);
            Directory.CreateDirectory(DevicesDir);
        }

        InitDir(SlithinDir);
        InitDir(DevicesDir);
    }

    public void InitDeviceDirectory()
    {
        if (!Directory.Exists(ConfigBaseDir))
        {
            Directory.CreateDirectory(ConfigBaseDir);
            Directory.CreateDirectory(DevicesDir);
            Directory.CreateDirectory(TemplatesDir);
            Directory.CreateDirectory(NotebooksDir);
            Directory.CreateDirectory(CustomScreensDir);
            Directory.CreateDirectory(BackupsDir);

            File.WriteAllText(Path.Combine(ConfigBaseDir, "templates.json"), "{\"templates\": []}");
        }

        InitDir(TemplatesDir);
        InitDir(NotebooksDir);
        InitDir(BackupsDir);
        InitDir(CustomScreensDir);
    }

    public void ReLink(string deviceName)
    {
        ConfigBaseDir = Path.Combine(DevicesDir, deviceName);
    }

    private void InitDir(string dir)
    {
        var di = new DirectoryInfo(dir);

        if (!di.Exists)
        {
            di.Create();
        }
    }
}
