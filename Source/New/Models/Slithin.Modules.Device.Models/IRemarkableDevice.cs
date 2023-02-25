using Renci.SshNet.Common;
using Slithin.Entities;

namespace Slithin.Modules.Device.Models;

public interface IRemarkableDevice
{
    event EventHandler<ScpDownloadEventArgs> Downloading;

    event EventHandler<ScpUploadEventArgs> Uploading;

    void Connect(IPAddress ip, string password);

    void Reload();

    void Disconned();

    void Download(string path, FileInfo fileInfo);

    void Upload(FileInfo fileInfo, string path);

    void Upload(DirectoryInfo dirInfo, string path);

    IReadOnlyList<FileFetchResult> FetchFilesWithModified(string directory, string searchPattern = "*.*", SearchOption searchOption = SearchOption.AllDirectories);

    IReadOnlyList<FileFetchResult> FetchedNotebooks { get; }
    IReadOnlyList<FileFetchResult> FetchedTemplates { get; }
    IReadOnlyList<FileFetchResult> FetchedScreens { get; }

    CommandResult RunCommand(string cmd);
}

public struct FileFetchResult
{
    public string ShortPath { get; init; }
    public string FullPath { get; init; }
    public long LastModified { get; init; }
}
