using Slithin.Messages;

namespace Slithin.Core.MessageHandlers
{
    public class PostActionMessageHandler : IMessageHandler<PostActionMessage>
    {
        public void HandleMessage(PostActionMessage message)
        {
            message.Action();
        }
    }
}
