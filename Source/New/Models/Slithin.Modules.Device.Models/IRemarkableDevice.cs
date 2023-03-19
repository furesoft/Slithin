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
    /// <summary>
    /// Event that triggers when a file/directory is downloading
    /// </summary>
    event EventHandler<ScpDownloadEventArgs> Downloading;

    /// <summary>
    /// Event that triggers when a file/directory is uploading
    /// </summary>
    event EventHandler<ScpUploadEventArgs> Uploading;

    IReadOnlyList<FileFetchResult> FetchedNotebooks { get; }

    IReadOnlyList<FileFetchResult> FetchedTemplates { get; }

    IReadOnlyList<FileFetchResult> FetchedScreens { get; }

    /// <summary>
    /// Initiate the connection to the device
    /// </summary>
    /// <param name="ip">the ip address with the port</param>
    /// <param name="password"></param>
    void Connect(IPAddress ip, string password);

    /// <summary>
    /// Reload xochitl on the device
    /// </summary>
    void Reload();

    /// <summary>
    /// Close the ssh connection
    /// </summary>
    void Disconnect();

    /// <summary>
    /// Download a file from the device to the filesystem
    /// </summary>
    /// <param name="path"></param>
    /// <param name="fileInfo"></param>
    void Download(string path, FileInfo fileInfo);

    /// <summary>
    /// Upload a file to the device
    /// </summary>
    /// <param name="fileInfo"></param>
    /// <param name="path"></param>
    void Upload(FileInfo fileInfo, string path);

    /// <summary>
    /// Upload a directory to the device
    /// </summary>
    /// <param name="dirInfo"></param>
    /// <param name="path"></param>
    void Upload(DirectoryInfo dirInfo, string path);

    /// <summary>
    /// Method For Getting all filenames with the modified option. Used for synchronisation.
    /// </summary>
    /// <param name="directory"></param>
    /// <param name="searchPattern"></param>
    /// <param name="searchOption"></param>
    /// <returns></returns>
    IReadOnlyList<FileFetchResult> FetchFilesWithModified(string directory, string searchPattern = "*.*", SearchOption searchOption = SearchOption.AllDirectories);

    /// <summary>
    /// Run a bash command on the device
    /// </summary>
    /// <param name="cmd"></param>
    /// <returns>Either the output stream as string or an error as exception</returns>
    Result<string> RunCommand(string cmd);

    /// <summary>
    /// Check if an ip address is reachable
    /// </summary>
    /// <param name="ip"></param>
    /// <returns></returns>
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
