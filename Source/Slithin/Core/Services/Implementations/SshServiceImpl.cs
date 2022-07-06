using System;
using System.IO;
using Renci.SshNet;
using Renci.SshNet.Common;

namespace Slithin.Core.Services.Implementations;

public class SshServiceImpl : ISSHService
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

    public ConnectionInfo ConnectionInfo => _client.ConnectionInfo;

    public static ISSHService New(SshClient client, ScpClient scp)
    {
        var service = new SshServiceImpl();
        service._client = client;
        service._scp = scp;

        return service;
    }

    public void Dispose()
    {
        _client.Disconnect();
        _scp.Disconnect();

        _client.Dispose();
        _scp.Dispose();
    }

    public void Download(string file, FileInfo dest)
    {
        TryReconnect();

        _scp.Download(file, dest);
    }

    public void Download(string file, DirectoryInfo dest)
    {
        TryReconnect();

        _scp.Download(file, dest);
    }

    public void Download(string file, Stream dest)
    {
        TryReconnect();

        _scp.Download(file, dest);
    }

    public SshCommand RunCommand(string cmd)
    {
        TryReconnect();

        return _client.RunCommand(cmd);
    }

    public void Upload(FileInfo file, string path)
    {
        TryReconnect();

        _scp.Upload(file, path);
    }

    public void Upload(Stream strm, string file)
    {
        TryReconnect();

        _scp.Upload(strm, file);
    }

    public void Upload(DirectoryInfo dir, string path)
    {
        TryReconnect();

        _scp.Upload(dir, path);
    }

    private void TryReconnect()
    {
        if (!_client.IsConnected)
        {
            _client.Connect();
        }
        if (!_scp.IsConnected)
        {
            _scp.Connect();
        }
    }
}
