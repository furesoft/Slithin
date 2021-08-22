using System;
using Actress;
using Slithin.Core.MessageHandlers;
using Slithin.Messages;

namespace Slithin.Core.Services.Implementations
{
    public class MailboxServiceImpl : IMailboxService
    {
        private readonly MessageRouter _messageRouter;
        private MailboxProcessor<AsynchronousMessage> _mailbox;

        public MailboxServiceImpl(MessageRouter messageRouter)
        {
            _messageRouter = messageRouter;
        }

        public void Init()
        {
            _mailbox = MailboxProcessor.Start<AsynchronousMessage>(
                async (_) =>
                {
                    while (true)
                    {
                        var msg = await _.Receive();

                        _messageRouter.Route(msg);
                    }
                }
            );
        }

        public void InitMessageRouter()
        {
            _messageRouter.Register(ServiceLocator.Container.Resolve<SyncMessageHandler>());

            _messageRouter.Register(ServiceLocator.Container.Resolve<InitStorageMessageHandler>());

            _messageRouter.Register(ServiceLocator.Container.Resolve<CheckForUpdatesMessageHandler>());

            _messageRouter.Register(ServiceLocator.Container.Resolve<AttentionRequiredMessageHandler>());

            _messageRouter.Register(ServiceLocator.Container.Resolve<PostActionMessageHandler>());

            _messageRouter.Register(ServiceLocator.Container.Resolve<DownloadNotebooksMessageHandler>());

            _messageRouter.Register(ServiceLocator.Container.Resolve<InitNotebookMessageHandler>());
        }

        public void Post(AsynchronousMessage msg)
        {
            _mailbox.Post(msg);
        }

        public void PostAction(Action p)
        {
            _mailbox.Post(new PostActionMessage(p));
        }
    }
}
