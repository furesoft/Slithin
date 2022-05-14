using System.Net;

namespace Slithin.Core.Services;

public interface IServiceDiscovery
{

    string[] Discover();
    bool PingDevice(IPAddress address);

}



