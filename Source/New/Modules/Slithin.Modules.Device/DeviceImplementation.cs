using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text;
using AuroraModularis.Core;
using DotNext;
using Renci.SshNet;
using Renci.SshNet.Common;
using Slithin.Entities;
using Slithin.Modules.Device.Models;

namespace Slithin.Modules.Device;

internal class DeviceImplementation : IRemarkableDevice
{
    private SshClient _client;
    private ScpClient _scp;

    public event EventHandler<ScpDownloadEventArgs> Downloading
    {
        add
        {
            _scp.Downloading += value;
        }
        remove
        {
            _scp.Downloading -= value;
        }
    }

    public event EventHandler<ScpUploadEventArgs> Uploading
    {
        add
        {
            _scp.Uploading += value;
        }
        remove
        {
            _scp.Uploading -= value;
        }
    }

    public IReadOnlyList<FileFetchResult> FetchedNotebooks => FetchFilesWithModified(ServiceContainer.Current.Resolve<DevicePathList>().Notebooks);

    public IReadOnlyList<FileFetchResult> FetchedTemplates => FetchFilesWithModified(ServiceContainer.Current.Resolve<DevicePathList>().Templates);

    public IReadOnlyList<FileFetchResult> FetchedScreens => FetchFilesWithModified(ServiceContainer.Current.Resolve<DevicePathList>().Screens, "*.png", SearchOption.TopDirectoryOnly);

    public void Connect(IPAddress ip, string password)
    {
        _client = new(ip.Address, ip.Port, "root", password);
        _client.Connect();

        _scp = new(ip.Address, ip.Port, "root", password);
        _scp.Connect();
    }

    public void Disconnect()
    {
        _client.Disconnect();
    }

    public void Download(string path, FileInfo fileInfo)
    {
        fileInfo.Directory.Create();

        _scp.Download(path, fileInfo);
    }

    public IReadOnlyList<FileFetchResult> FetchFilesWithModified(string directory, string searchPattern = "*.*", SearchOption searchOption = SearchOption.AllDirectories)
    {
        var expandedSearchOption = searchOption == SearchOption.TopDirectoryOnly ? "-maxdepth 1" : "";
        var findCmdResult = RunCommand($"find {directory} {expandedSearchOption} -type f -name '{searchPattern}'");
        var lastModResult = RunCommand($"find {directory} {expandedSearchOption} -type f -name '{searchPattern}' | xargs stat -c \"%Y\"");

        if (!findCmdResult.IsSuccessful || !lastModResult.IsSuccessful)
        {
            Debug.WriteLine($"[findCmd]: {findCmdResult.Error.Message}");
            Debug.WriteLine($"[lastMod]: {lastModResult.Error.Message}");
        }

        var findCmdOutput = findCmdResult.Value.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        var lastModOutput = lastModResult.Value.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        var result = new List<FileFetchResult>();

        for (int i = 0; i < findCmdOutput.Length; i++)
        {
            result.Add(new()
            {
                ShortPath = findCmdOutput[i].Substring(directory.Length),
                FullPath = findCmdOutput[i],
                LastModified = long.Parse(lastModOutput[i]),
            });
        }

        return result;
    }

    /// <inheritdoc cref="IRemarkableDevice.Reload" />
    public void Reload()
    {
        _client.RunCommand("systemctl restart xochitl");
    }

    /// <inheritdoc cref="IRemarkableDevice.RunCommand" />
    public Result<string> RunCommand(string cmd)
    {
        var result = _client.RunCommand(cmd);

        if (!string.IsNullOrEmpty(result.Error))
        {
            return Result.FromException<string>(new(result.Error));
        }
        
        return result.Result;
    }

    public async Task<bool> Ping(IPAddress ip)
    {
        var pingSender = new Ping();

        var data = new string('a', 32);
        var buffer = Encoding.ASCII.GetBytes(data);

        const int Timeout = 10000;

        var options = new PingOptions(64, true);

        var reply = pingSender.Send(ip.Address, Timeout, buffer,
            options);

        return reply.Status == IPStatus.Success;
    }

    public void Upload(FileInfo fileInfo, string path)
    {
        _scp.Upload(fileInfo, path);
    }

    public void Upload(DirectoryInfo dirInfo, string path)
    {
        _scp.Upload(dirInfo, path);
    }
}
