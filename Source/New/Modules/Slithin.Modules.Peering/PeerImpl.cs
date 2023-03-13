using Slithin.Modules.Peering.Models;

namespace Slithin.Modules.Peering;

public class PeerImpl : IPeer
{
    public void Init()
    {
        
    }

    public void Broadcast<T>(T message)
    {
        
    }

    public event Action<object>? OnMessageReceived;
}
