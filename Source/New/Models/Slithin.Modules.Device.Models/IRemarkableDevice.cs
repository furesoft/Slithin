using AuroraModularis.Core;
using DotNext;
using Renci.SshNet.Common;
using Slithin.Entities;
using Slithin.Modules.Repository.Models;

namespace Slithin.Modules.Device.Models;

/// <summary>
/// Interface to communicate with a device
/// </summary>
public interface IRemarkableDevice
{
    event EventHandler<ScpDownloadEventArgs> Downloading;

    event EventHandler<ScpUploadEventArgs> Uploading;

    IReadOnlyList<FileFetchResult> FetchedNotebooks { get; }

    IReadOnlyList<FileFetchResult> FetchedTemplates { get; }

    IReadOnlyList<FileFetchResult> FetchedScreens { get; }

    void Connect(IPAddress ip, string password);

    /// <summary>
    /// Reload xochitl on the device
    /// </summary>
    void Reload();

    /// <summary>
    /// Close the ssh connection
    /// </summary>
    void Disconnect();

    void Download(string path, FileInfo fileInfo);

    void Upload(FileInfo fileInfo, string path);

    void Upload(DirectoryInfo dirInfo, string path);

    IReadOnlyList<FileFetchResult> FetchFilesWithModified(string directory, string searchPattern = "*.*", SearchOption searchOption = SearchOption.AllDirectories);

    /// <summary>
    /// Run a bash command on the device
    /// </summary>
    /// <param name="cmd"></param>
    /// <returns>Either the output stream as string or an error as exception</returns>
    Result<string> RunCommand(string cmd);

    Task<bool> Ping(IPAddress ip);

    public async Task<bool> Ping(string ip)
    {
        return await Ping(IPAddress.Parse(ip));
    }

    /// <summary>
    /// Check if device is reachable
    /// </summary>
    /// <returns></returns>
    public async Task<bool> Ping()
    {
        var loginService = ServiceContainer.Current.Resolve<ILoginService>();

        return await Ping(loginService.GetCurrentCredential().IP);
    }
}
