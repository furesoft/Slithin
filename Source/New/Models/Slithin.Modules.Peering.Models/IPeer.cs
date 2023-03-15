namespace Slithin.Modules.Peering.Models;

/// <summary>
/// A service to work with other peers
/// </summary>
public interface IPeer
{
    void Init();

    void Broadcast<T>(T message);

    event Action<object> OnMessageReceived;

    /// <summary>
    /// Register a message handler. Will be invoked after a message is received.
    /// </summary>
    /// <typeparam name="TMessageType"></typeparam>
    /// <typeparam name="THandlerType"></typeparam>
    void RegisterMessageHandler<TMessageType, THandlerType>() 
        where THandlerType : IMessageHandler<TMessageType>, new();
}
