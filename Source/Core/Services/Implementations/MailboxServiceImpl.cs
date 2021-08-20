using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Actress;
using Newtonsoft.Json;
using Renci.SshNet;
using Renci.SshNet.Common;
using Slithin.Controls;
using Slithin.Core.MessageHandlers;
using Slithin.Core.Remarkable;
using Slithin.Core.Sync;
using Slithin.Core.Sync.Repositorys;
using Slithin.Messages;
using Slithin.Tools;

namespace Slithin.Core.Services.Implementations
{
    public class MailboxServiceImpl : IMailboxService
    {
        private readonly DeviceRepository _device;
        private readonly ILoadingService _loadingService;
        private readonly LocalRepository _local;
        private readonly MessageRouter _messageRouter;
        private readonly IPathManager _pathManager;
        private readonly SynchronisationService _syncService;
        private MailboxProcessor<AsynchronousMessage> _mailbox;

        public MailboxServiceImpl(IPathManager pathManager,
                                  SynchronisationService syncService,
                                  DeviceRepository device,
                                  LocalRepository local,
                                  MessageRouter messageRouter,
                                  ILoadingService loadingService)
        {
            _pathManager = pathManager;
            _syncService = syncService;
            _device = device;
            _local = local;
            _messageRouter = messageRouter;
            _loadingService = loadingService;
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
            var client = ServiceLocator.Container.Resolve<SshClient>();
            var scp = ServiceLocator.Container.Resolve<ScpClient>();

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
