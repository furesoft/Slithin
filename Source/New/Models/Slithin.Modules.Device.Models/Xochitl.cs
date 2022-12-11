using AuroraModularis.Logging.Models;
using IniParser;
using IniParser.Model;
using Slithin.Modules.Device.Models;
using Slithin.Modules.Repository.Models;

namespace Slithin.Modules.Device;

public class Xochitl
{
    private readonly ILogger _logger;
    private readonly IRemarkableDevice _remarkableDevice;
    private readonly IPathManager _pathManager;
    private IniData _data;
    private FileIniDataParser _ini;

    public Xochitl(IPathManager pathManager, ILogger logger, IRemarkableDevice remarkableDevice)
    {
        _pathManager = pathManager;
        _logger = logger;
        _remarkableDevice = remarkableDevice;
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
        var fileInfo = new FileInfo(Path.Combine(_pathManager.ConfigBaseDir, "xochitl.conf"));

        _logger.Info("Downloading 'xochitl.conf'");

        if (fileInfo.Exists)
        {
            fileInfo.Delete();
        }

        _remarkableDevice.Download("/home/root/.config/remarkable/xochitl.conf", fileInfo);

        _ini = new FileIniDataParser();

        _data = _ini.ReadFile(fileInfo.FullName);
    }

    public void ReloadDevice()
    {
        var result = _remarkableDevice.RunCommand("systemctl restart xochitl");

        if (result.Error != null)
        {
            _logger.Error(result.Error);
        }
    }

    public void Save()
    {
        //CustomIniSerializer.WriteFile(Path.Combine(_pathManager.ConfigBaseDir, "xochitl.conf"), _data);
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
        var fileInfo = new FileInfo(Path.Combine(_pathManager.ConfigBaseDir, "xochitl.conf"));

        _remarkableDevice.Upload(fileInfo, "/home/root/.config/remarkable/xochitl.conf");
    }
}
