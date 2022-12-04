namespace Slithin.Modules.Device.Models;

public interface IRemarkableDevice
{
    void Connect(string ip, string password);

    void Reload();

    void Disconned();
}
