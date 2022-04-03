using System;
using System.IO;
using Actress;
using Serilog;
using Slithin.Messages;
using Slithin.Core.Messaging;
using Slithin.MessageHandlers;

namespace Slithin.Core.Services.Implementations;

public class MailboxServiceImpl : IMailboxService
{
    private readonly ILocalisationService _localisationService;
    private readonly ILogger _logger;
    private readonly MessageRouter _messageRouter;
    private readonly IPathManager _pathManager;
    private MailboxProcessor<AsynchronousMessage> _mailbox;

    public MailboxServiceImpl(
        MessageRouter messageRouter,
        ILogger logger,
        IPathManager pathManager,
        ILocalisationService localisationService)
    {
        _messageRouter = messageRouter;
        _logger = logger;
        _pathManager = pathManager;
        _localisationService = localisationService;
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

        NotificationService.ShowError(
            _localisationService.GetStringFormat(
                "An Error occured. See ({0})", Path.Combine(_pathManager.SlithinDir, "log.txt")));
    }
}
