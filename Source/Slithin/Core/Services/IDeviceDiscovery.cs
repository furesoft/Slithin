using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Slithin.Core.Services;

public interface IDeviceDiscovery
{
    Task<Dictionary<string, IPAddress>> Discover();

    bool PingDevice(IPAddress address);
}
