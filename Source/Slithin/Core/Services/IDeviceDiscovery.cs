using System.Net;

namespace Slithin.Core.Services;

public interface IDeviceDiscovery
{
    IPAddress Discover();

    bool PingDevice(IPAddress address);
}
