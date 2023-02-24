﻿using Renci.SshNet;
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

    public void Connect(IPAddress ip, string password)
    {
        _client = new(ip.Address, ip.Port, "root", password);
        _client.Connect();

        _scp = new(ip.Address, ip.Port, "root", password);
        _scp.Connect();
    }

    public void Disconned()
    {
        _client.Disconnect();
    }

    public void Download(string path, FileInfo fileInfo)
    {
        _scp.Download(path, fileInfo);
    }

    public IReadOnlyList<(string, long)> FetchFilesWithModified(string directory)
    {
        var output = _client.RunCommand($"find {directory} -type f -not -path '*/\\.*'; find {directory} -type f -not -path '*/\\.*' | xargs stat -c \"%Y\"").Result.Split('\n');
        int middle = output.Length / 2;
        var result = new List<(string, long)>();
        for (int i = 0; i < middle; i++)
        {
            result.Add((output[i].Substring(directory.Length + 1), long.Parse(output[i + middle])));
        }
        return result;
    }

    public void Reload()
    {
        _client.RunCommand("systemctl restart xochitl");
    }

    public CommandResult RunCommand(string cmd)
    {
        var result = _client.RunCommand(cmd);

        return new(result.Error, result.Result);
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
