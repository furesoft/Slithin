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
}
