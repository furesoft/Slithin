using Slithin.Entities;

namespace Slithin.Modules.Device.Models;

public interface IRemarkableDevice
{
    void Connect(IPAddress ip, string password);

    void Reload();

    void Disconned();

    void Download(string path, FileInfo fileInfo);

    void Upload(FileInfo fileInfo, string path);

    CommandResult RunCommand(string cmd);
}
