using System.Collections.Generic;
using System.IO;
using System.Linq;
using IniParser;
using IniParser.Model;
using Renci.SshNet;
using Serilog;
using Slithin.Core.Services;

namespace Slithin.Core.Remarkable;

public class Xochitl
{
    private readonly SshClient _client;
    private readonly ILogger _logger;
    private readonly IPathManager _pathManager;
    private readonly ScpClient _scp;
    private IniData _data;
    private FileIniDataParser _ini;

    public Xochitl(ScpClient scp, IPathManager pathManager, ILogger logger, SshClient client)
    {
        _scp = scp;
        _pathManager = pathManager;
        _logger = logger;
        _client = client;
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
        return _data[section][key];
    }

    public string[] GetShareEmailAddresses()
    {
        var str = GetProperty("ShareEmailAddresses", "General");

        return str.Split(',').Select(_ => _.Trim()).ToArray();
    }

    public string GetToken(string key, string section)
    {
        var value = GetProperty(key, section);

        if (value.StartsWith("@ByteArray"))
        {
            var start = value.IndexOf("(") + 1;
            var end = value.IndexOf(")");

            return value.Substring(start, end - start);
        }

        return null;
    }

    public void Init()
    {
        var fileInfo = new FileInfo(Path.Combine(_pathManager.ConfigBaseDir, "xochitl.conf"));

        _logger.Information("Downloading 'xochitl.conf'");

        if (fileInfo.Exists)
        {
            fileInfo.Delete();
        }

        _scp.Download("/home/root/.config/remarkable/xochitl.conf", fileInfo);

        _ini = new FileIniDataParser();

        _data = _ini.ReadFile(fileInfo.FullName);
    }

    public void ReloadDevice()
    {
        var result = _client.RunCommand("systemctl restart xochitl");

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
        var fileInfo = new FileInfo(Path.Combine(_pathManager.ConfigBaseDir, "xochitl.conf"));

        NotificationService.Show("Uploading xochitl.conf");
        _scp.Upload(fileInfo, "/home/root/.config/remarkable/xochitl.conf");
    }
}
