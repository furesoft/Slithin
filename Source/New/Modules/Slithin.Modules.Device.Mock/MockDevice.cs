using AuroraModularis.Core;
using DotNext;
using Renci.SshNet.Common;
using Slithin.Entities;
using Slithin.Modules.Device.Models;
using Zio;
using Zio.FileSystems;

namespace Slithin.Modules.Device.Mock;

internal class MockDevice : IRemarkableDevice
{
    private readonly ServiceContainer _container;
    private IFileSystem _filesystem;

    public MockDevice(ServiceContainer container)
    {
        _container = container;
    }

    ~MockDevice()
    {
        Disconnect();
    }

    public event EventHandler<ScpDownloadEventArgs> Downloading;

    public event EventHandler<ScpUploadEventArgs> Uploading;

    public void Connect(IPAddress ip, string password)
    {
        _filesystem = new ZipArchiveFileSystem(new System.IO.Compression.ZipArchive(File.Open("device.zip", FileMode.Open), System.IO.Compression.ZipArchiveMode.Update));

        var xochitl = _container.Resolve<IXochitlService>();
        xochitl.Init();
    }

    public void Disconnect()
    {
        _filesystem.Dispose();
    }

    public void Download(string path, FileInfo fileInfo)
    {
        using var zipStream = _filesystem.OpenFile(path, FileMode.Open, FileAccess.Read);
        using var fileStream = File.OpenWrite(fileInfo.FullName);

        zipStream.CopyTo(fileStream);
    }

    public IReadOnlyList<FileFetchResult> FetchFilesWithModified(string directory, string searchPattern = "*.*", SearchOption searchOption = SearchOption.AllDirectories)
    {
        var list = new List<FileFetchResult>();
        foreach (FileEntry file in _filesystem.EnumerateFileEntries(directory, searchPattern, searchOption))
        {
            list.Add(new()
            {
                ShortPath = file.FullName.Substring(directory.Length),
                FullPath = file.FullName,
                LastModified = file.LastWriteTime.Ticks,
            });
        }
        return list;
    }

    public IReadOnlyList<FileFetchResult> FetchedNotebooks => FetchFilesWithModified(ServiceContainer.Current.Resolve<DevicePathList>().Notebooks);

    public IReadOnlyList<FileFetchResult> FetchedTemplates => FetchFilesWithModified(ServiceContainer.Current.Resolve<DevicePathList>().Templates);

    public IReadOnlyList<FileFetchResult> FetchedScreens => FetchFilesWithModified(ServiceContainer.Current.Resolve<DevicePathList>().Screens, "*.png", SearchOption.TopDirectoryOnly);

    public void Reload()
    {
    }

    public Result<string> RunCommand(string cmd)
    {
        if (cmd == "grep '^REMARKABLE_RELEASE_VERSION' /usr/share/remarkable/update.conf")
        {
            return "2.13.5";
        }

        return Result.FromException<string>(new Exception());
    }

    public Task<bool> Ping(IPAddress ip)
    {
        return Task.FromResult(true);
    }

    public void Upload(FileInfo fileInfo, string path)
    {
        using var fileStream = File.Open(fileInfo.FullName, FileMode.Open);
        using var zipStream = _filesystem.CreateFile(path);

        fileStream.CopyTo(zipStream);

        _filesystem.Dispose();
        Connect(default, string.Empty);
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
