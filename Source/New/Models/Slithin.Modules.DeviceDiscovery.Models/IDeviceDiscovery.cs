using System.Net;

namespace Slithin.Modules.DeviceDiscovery.Models;

public interface IDeviceDiscovery
{
    IPAddress Discover();

    bool PingDevice(IPAddress address);
}
