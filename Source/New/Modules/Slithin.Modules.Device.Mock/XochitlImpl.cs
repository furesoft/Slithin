using AuroraModularis.Core;
using AuroraModularis.Logging.Models;
using IniParser;
using IniParser.Model;
using Slithin.Modules.Device.Models;
using Slithin.Modules.Repository.Models;

namespace Slithin.Modules.Device.Mock;

internal class XochitlImpl : IXochitlService
{
    private readonly Container _container;
    private IniData _data;
    private FileIniDataParser _ini;

    public XochitlImpl(Container container)
    {
        _container = container;
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
        var pathManager = _container.Resolve<IPathManager>();
        var logger = _container.Resolve<ILogger>();
        var remarkableDevice = _container.Resolve<IRemarkableDevice>();

        var fileInfo = new FileInfo(Path.Combine(pathManager.ConfigBaseDir, "xochitl.conf"));

        logger.Info("Downloading 'xochitl.conf'");

        if (fileInfo.Exists)
        {
            fileInfo.Delete();
        }

        remarkableDevice.Download("/home/root/.config/remarkable/xochitl.conf", fileInfo);

        _ini = new FileIniDataParser();

        _data = _ini.ReadFile(fileInfo.FullName);
    }

    public void Save()
    {
        var pathManager = _container.Resolve<IPathManager>();
        var device = _container.Resolve<IRemarkableDevice>();

        CustomIniSerializer.WriteFile(Path.Combine(pathManager.ConfigBaseDir, "xochitl.conf"), _data);
        Upload();

        device.Reload();
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
        var remarkableDevice = _container.Resolve<IRemarkableDevice>();
        var pathManager = _container.Resolve<IPathManager>();

        var fileInfo = new FileInfo(Path.Combine(pathManager.ConfigBaseDir, "xochitl.conf"));

        remarkableDevice.Upload(fileInfo, "/home/root/.config/remarkable/xochitl.conf");
    }
}
