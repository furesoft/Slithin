namespace Slithin.Modules.Peering.Models;

/// <summary>
/// An interface to work with messages sent by another peer
/// </summary>
/// <typeparam name="T">The message type</typeparam>
public interface IMessageHandler<in T>
{
    /// <summary>
    /// Do something with the message
    /// </summary>
    /// <param name="message"></param>
    void Handle(T message);
}
