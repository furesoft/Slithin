using AuroraModularis.Core;
using Renci.SshNet.Common;
using Slithin.Entities;
using Slithin.Modules.Device.Models;
using Zio;
using Zio.FileSystems;

namespace Slithin.Modules.Device.Mock;

internal class MockDevice : IRemarkableDevice
{
    private readonly Container _container;
    private IFileSystem _filesystem;

    public MockDevice(Container container)
    {
        _container = container;
    }

    ~MockDevice()
    {
        Disconned();
    }

    public event EventHandler<ScpDownloadEventArgs> Downloading;

    public event EventHandler<ScpUploadEventArgs> Uploading;

    public void Connect(IPAddress ip, string password)
    {
        _filesystem = new ZipArchiveFileSystem(new System.IO.Compression.ZipArchive(File.Open("device.zip", FileMode.Open), System.IO.Compression.ZipArchiveMode.Update));

        var xochitl = _container.Resolve<IXochitlService>();
        xochitl.Init();
    }

    public void Disconned()
    {
        _filesystem.Dispose();
    }

    public void Download(string path, FileInfo fileInfo)
    {
        using var zipStream = _filesystem.OpenFile(path, FileMode.Open, FileAccess.Read);
        using var fileStream = File.OpenWrite(fileInfo.FullName);

        zipStream.CopyTo(fileStream);
    }

    public void Reload()
    {
    }

    public CommandResult RunCommand(string cmd)
    {
        if (cmd == "grep '^REMARKABLE_RELEASE_VERSION' /usr/share/remarkable/update.conf")
        {
            return new(null, "2.13.5");
        }

        return new(null, "");
    }

    public void Upload(FileInfo fileInfo, string path)
    {
        using var fileStream = File.Open(fileInfo.FullName, FileMode.Open);
        using var zipStream = _filesystem.CreateFile(path);

        fileStream.CopyTo(zipStream);

        _filesystem.Dispose();
        Connect(default, null);
    }

    public void Upload(DirectoryInfo dirInfo, string path)
    {
        if (!_filesystem.DirectoryExists(path))
        {
            _filesystem.CreateDirectory(path);
        }

        foreach (var file in dirInfo.GetFiles())
        {
            Upload(file, (string)((UPath)path / file.Name));
        }

        foreach (var dir in dirInfo.GetDirectories())
        {
            Upload(dir, (string)((UPath)path / dir.Name));
        }
    }
}
