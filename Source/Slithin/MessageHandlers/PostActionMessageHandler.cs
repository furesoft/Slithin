using Slithin.Messages;
using Slithin.Core.Messaging;

namespace Slithin.MessageHandlers;

public class PostActionMessageHandler : IMessageHandler<PostActionMessage>
{
    public void HandleMessage(PostActionMessage message)
    {
        message.Action();
    }
}
