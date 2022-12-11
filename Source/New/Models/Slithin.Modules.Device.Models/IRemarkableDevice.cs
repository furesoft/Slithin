namespace Slithin.Modules.Device.Models;

public interface IRemarkableDevice
{
    void Connect(string ip, string password);

    void Reload();

    void Disconned();

    void Download(string path, FileInfo fileInfo);

    void Upload(FileInfo fileInfo, string path);

    CommandResult RunCommand(string cmd);
}
