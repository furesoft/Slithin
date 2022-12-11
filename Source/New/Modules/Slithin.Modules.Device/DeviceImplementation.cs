﻿using Renci.SshNet;
using Slithin.Modules.Device.Models;

namespace Slithin.Modules.Device;

internal class DeviceImplementation : IRemarkableDevice
{
    private SshClient _client;
    private ScpClient _scp;

    public void Connect(string ip, string password)
    {
        _client = new(ip, 22, "root", password);
        _client.Connect();

        _scp = new(ip, 22, "root", password);
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

    public Xochitl GetXochitl()
    {
        return new Xochitl(null, null, this);
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
}
