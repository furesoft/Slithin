using System.IO;
using IniParser;
using IniParser.Model;
using Renci.SshNet;
using Serilog;
using Slithin.Core.Services;

namespace Slithin.Core.Remarkable;

public class Xochitl
{
    private readonly ScpClient _client;
    private readonly ILogger _logger;
    private readonly IPathManager _pathManager;
    private IniData _data;

    public Xochitl(ScpClient client, IPathManager pathManager, ILogger logger)
    {
        _client = client;
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
        return _data[section][key];
    }

    public void Init()
    {
        var fileInfo = new FileInfo(Path.Combine(_pathManager.ConfigBaseDir, "xochitl.conf"));

        _logger.Information("Downloading 'xochitl.conf'");
        _client.Download("/home/root/.config/remarkable/xochitl.conf", fileInfo);

        var parser = new FileIniDataParser();

        _data = parser.ReadFile(fileInfo.FullName);
    }
}
