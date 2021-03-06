using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IniParser;
using IniParser.Model;
using Serilog;
using Slithin.Core.Services;

namespace Slithin.Core.Remarkable;

public class Xochitl
{
    private readonly ILogger _logger;
    private readonly IPathManager _pathManager;
    private IniData _data;
    private FileIniDataParser _ini;

    public Xochitl(IPathManager pathManager, ILogger logger)
    {
        _pathManager = pathManager;
        _logger = logger;
    }

    public bool GetIsBeta()
    {
        var str = GetProperty("BetaProgram", "General");

        if (!string.IsNullOrEmpty(str))
        {
            return bool.Parse(str);
        }

        return false;
    }

    public string GetProperty(string key, string section)
    {
        if (!_data.Sections.ContainsSection(section) || !_data[section].ContainsKey(key))
        {
            return string.Empty;
        }

        return _data[section][key];
    }

    public string[] GetShareEmailAddresses()
    {
        var str = GetProperty("ShareEmailAddresses", "General");
        var splt = str?.Split(',');
        if (splt.Any())
        {
            return splt.Select(_ => _.Trim()).ToArray();
        }

        return Array.Empty<string>();
    }

    public string GetToken(string key, string section)
    {
        var value = GetProperty(key, section);

        if (value.StartsWith("@ByteArray"))
        {
            var start = value.IndexOf("(") + 1;
            var end = value.IndexOf(")");

            return value[start..end];
        }

        return null;
    }

    public void Init()
    {
        var _ssh = ServiceLocator.Container.Resolve<ISSHService>();

        var fileInfo = new FileInfo(Path.Combine(_pathManager.ConfigBaseDir, "xochitl.conf"));

        _logger.Information("Downloading 'xochitl.conf'");

        if (fileInfo.Exists)
        {
            fileInfo.Delete();
        }

        _ssh.Download("/home/root/.config/remarkable/xochitl.conf", fileInfo);

        _ini = new FileIniDataParser();

        _data = _ini.ReadFile(fileInfo.FullName);
    }

    public void ReloadDevice()
    {
        var _ssh = ServiceLocator.Container.Resolve<ISSHService>();

        var result = _ssh.RunCommand("systemctl restart xochitl");

        if (result.ExitStatus != 0)
        {
            _logger.Error(result.Error);
        }
    }

    public void Save()
    {
        CustomIniSerializer.WriteFile(Path.Combine(_pathManager.ConfigBaseDir, "xochitl.conf"), _data);
        Upload();

        ReloadDevice();
    }

    public void SetPowerOffDelay(int value)
    {
        SetProperty("SuspendPowerOffDelay", "General", value.ToString());
        Save();
    }

    public void SetProperty(string key, string section, object value)
    {
        _data[section][key] = value.ToString();
    }

    public void SetShareMailAddresses(IEnumerable<string> mailAddresses)
    {
        var joinedMailList = string.Join(", ", mailAddresses);
        SetProperty("ShareEmailAddresses", "General", joinedMailList);
    }

    public void Upload()
    {
        var _ssh = ServiceLocator.Container.Resolve<ISSHService>();

        var fileInfo = new FileInfo(Path.Combine(_pathManager.ConfigBaseDir, "xochitl.conf"));

        NotificationService.Show("Uploading xochitl.conf");
        _ssh.Upload(fileInfo, "/home/root/.config/remarkable/xochitl.conf");
    }
}
