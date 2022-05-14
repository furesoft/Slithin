using System.Collections.Concurrent;
using System.Net;

namespace Slithin.Core.Services.Implementations;

public class ServiceDiscoveryImpl : ICServiceDiscovery
{

    public string[] Discover() {
        var address = GetLocalAddress();
        var subnet = address.

        return []
    }

    public bool PingDevice(IPAddress address) {
        
        return false
    }

    private IPAddress GetLocalAddress() {
        var host = Dns.GetHostEntry(Dns.GetHostName());

        foreach (var ip in host.AddressList) {
            if (ip.AddressFamily == AddressFamily.InterNetwork) {
                return ip.Address;
            }
        }

        throw new Exception("No Network adapters with Ipv4 address in the system!");
    }

}
