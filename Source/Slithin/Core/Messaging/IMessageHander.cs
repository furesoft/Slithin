using Slithin.Core;
namespace Slithin.Core.Messaging;

public interface IMessageHandler<in T>
    where T : AsynchronousMessage
{
    void HandleMessage(T message);
}
