using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using Slithin.Modules.BaseServices.Models;

namespace Slithin.Modules.BaseServices;

internal class DeviceDiscoveryImpl : IDeviceDiscovery
{
    public IPAddress? Discover()
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

        const int Timeout = 10000;

        var options = new PingOptions(64, true);

        var reply = pingSender.Send(address, Timeout, buffer,
            options);

        return reply.Status == IPStatus.Success;
    }
}
