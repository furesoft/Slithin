using AuroraModularis.Core;
using Renci.SshNet.Common;
using Slithin.Entities;
using Slithin.Modules.Repository.Models;

namespace Slithin.Modules.Device.Models;

public interface IRemarkableDevice
{
    event EventHandler<ScpDownloadEventArgs> Downloading;

    event EventHandler<ScpUploadEventArgs> Uploading;

    IReadOnlyList<FileFetchResult> FetchedNotebooks { get; }

    IReadOnlyList<FileFetchResult> FetchedTemplates { get; }

    IReadOnlyList<FileFetchResult> FetchedScreens { get; }

    void Connect(IPAddress ip, string password);

    void Reload();

    void Disconnect();

    void Download(string path, FileInfo fileInfo);

    void Upload(FileInfo fileInfo, string path);

    void Upload(DirectoryInfo dirInfo, string path);

    IReadOnlyList<FileFetchResult> FetchFilesWithModified(string directory, string searchPattern = "*.*", SearchOption searchOption = SearchOption.AllDirectories);

    CommandResult RunCommand(string cmd);

    Task<bool> Ping(IPAddress ip);

    public async Task<bool> Ping(string ip)
    {
        return await Ping(IPAddress.Parse(ip));
    }

    public async Task<bool> Ping()
    {
        var loginService = ServiceContainer.Current.Resolve<ILoginService>();

        return await Ping(loginService.GetCurrentCredential().IP);
    }
}
