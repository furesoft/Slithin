using System;
using Actress;
using Serilog;
using Slithin.Core.MessageHandlers;
using Slithin.Messages;

namespace Slithin.Core.Services.Implementations;

public class MailboxServiceImpl : IMailboxService
{
    private readonly ILogger _logger;
    private readonly MessageRouter _messageRouter;
    private MailboxProcessor<AsynchronousMessage> _mailbox;

    public MailboxServiceImpl(MessageRouter messageRouter, ILogger logger)
    {
        _messageRouter = messageRouter;
        _logger = logger;
    }

    public void Init()
    {
        _mailbox = MailboxProcessor.Start<AsynchronousMessage>(
            async _ =>
            {
                while (true)
                {
                    var msg = await _.Receive();

                    _messageRouter.Route(msg);
                }
            }
        );
        _mailbox.Errors.Subscribe(OnError);
    }

    public void InitMessageRouter()
    {
        _messageRouter.Register(ServiceLocator.Container.Resolve<SyncMessageHandler>());

        _messageRouter.Register(ServiceLocator.Container.Resolve<InitStorageMessageHandler>());

        _messageRouter.Register(ServiceLocator.Container.Resolve<CheckForUpdatesMessageHandler>());

        _messageRouter.Register(ServiceLocator.Container.Resolve<AttentionRequiredMessageHandler>());

        _messageRouter.Register(ServiceLocator.Container.Resolve<PostActionMessageHandler>());

        _messageRouter.Register(ServiceLocator.Container.Resolve<CollectSyncNotebooksMessageHandler>());

        _messageRouter.Register(ServiceLocator.Container.Resolve<InitNotebookMessageHandler>());

        _messageRouter.Register(ServiceLocator.Container.Resolve<DownloadSyncNotebooksMessageHandler>());
    }

    public void Post(AsynchronousMessage msg)
    {
        _mailbox.Post(msg);
    }

    public void PostAction(Action p)
    {
        _mailbox.Post(new PostActionMessage(p));
    }

    private void OnError(Exception obj)
    {
        _logger.Error(obj.ToString());
        NotificationService.Show("An Error occured. See log file");
    }
}
