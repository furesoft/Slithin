using System;
using System.IO;
using Renci.SshNet;
using Renci.SshNet.Common;

namespace Slithin.Core.Services;

public interface ISSHService : IDisposable
{
    event EventHandler<ScpDownloadEventArgs> Downloading;

    event EventHandler<ScpUploadEventArgs> Uploading;

    ConnectionInfo ConnectionInfo { get; }

    void Download(string file, FileInfo dest);

    void Download(string file, Stream dest);

    void Download(string file, DirectoryInfo dest);

    SshCommand RunCommand(string cmd);

    void Upload(FileInfo file, string path);

    void Upload(Stream strm, string path);

    void Upload(DirectoryInfo dir, string path);
}
