using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace Slithin.Core.Services.Implementations;

public class DeviceDiscoveryImpl : IDeviceDiscovery
{
    public IPAddress Discover()
    {
        return Dns.GetHostAddresses("remarkable")
            .First(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
    }

    //ToDo: Replace other Ping Calls with this
    public bool PingDevice(IPAddress address)
    {
        var pingSender = new Ping();

        var data = new string('a', 32);
        var buffer = Encoding.ASCII.GetBytes(data);

        var timeout = 10000;

        var options = new PingOptions(64, true);

        var reply = pingSender.Send(address, timeout, buffer,
            options);

        return reply.Status != IPStatus.Success;
    }
}
