namespace Slithin.Modules.Peering.Models;

public interface IPeer
{
    void Init();

    void Broadcast<T>(T message);

    event Action<object> OnMessageReceived;

    void RegisterMessageHandler<TMessageType, THandlerType>() 
        where THandlerType : IMessageHandler<TMessageType>, new();
}
