using Renci.SshNet;
using Slithin.Modules.Device.Models;

namespace Slithin.Modules.Device;

internal class DeviceImplementation : IRemarkableDevice
{
    private SshClient client;

    public void Connect(string ip, string password)
    {
        client = new(ip, 22, "root", password);
        client.Connect();
    }

    public void Disconned()
    {
        client.Disconnect();
    }

    public object GetXochitl()
    {
        throw new NotImplementedException();
    }

    public void Reload()
    {
        client.RunCommand("systemctl restart xochitl");
    }
}
