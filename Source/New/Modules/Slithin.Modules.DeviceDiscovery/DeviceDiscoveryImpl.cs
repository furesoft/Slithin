using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using Slithin.Modules.DeviceDiscovery.Models;

namespace Slithin.Modules.DeviceDiscovery;

public class DeviceDiscoveryImpl : IDeviceDiscovery
{
    public IPAddress Discover()
    {
        try
        {
            return Dns.GetHostAddresses("remarkable")
                .FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
        }
        catch
        {
            return null;
        }
    }

    public bool PingDevice(IPAddress address)
    {
        var pingSender = new Ping();

        var data = new string('a', 32);
        var buffer = Encoding.ASCII.GetBytes(data);

        var timeout = 10000;

        var options = new PingOptions(64, true);

        var reply = pingSender.Send(address, timeout, buffer,
            options);

        return reply.Status == IPStatus.Success;
    }
}
