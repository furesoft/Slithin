namespace Slithin.Core;

public interface IMessageHandler<in T>
    where T : AsynchronousMessage
{
    void HandleMessage(T message);
}