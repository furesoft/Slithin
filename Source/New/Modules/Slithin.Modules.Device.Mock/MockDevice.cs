using Slithin.Modules.Device.Models;
using Zio;
using Zio.FileSystems;

namespace Slithin.Modules.Device.Mock;

public class MockDevice : IRemarkableDevice
{
    private IFileSystem _filesystem;

    public void Connect(string ip, string password)
    {
        _filesystem = new ZipArchiveFileSystem(new System.IO.Compression.ZipArchive(File.Open("device.zip", FileMode.OpenOrCreate)));
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
        return new(null, "");
    }

    public void Upload(FileInfo fileInfo, string path)
    {
        using var fileStream = File.Open(fileInfo.FullName, FileMode.Open);
        using var zipStream = _filesystem.CreateFile(path);

        fileStream.CopyTo(zipStream);
    }
}
