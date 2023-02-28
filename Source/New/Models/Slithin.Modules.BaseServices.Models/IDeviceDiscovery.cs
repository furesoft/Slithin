using System.Net;

namespace Slithin.Modules.BaseServices.Models;

public interface IDeviceDiscovery
{
    IPAddress? Discover();
}
