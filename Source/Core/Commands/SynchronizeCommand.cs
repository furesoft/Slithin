using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using Renci.SshNet;
using Slithin.Core.Remarkable;
using Slithin.Core.Scripting;
using Slithin.Core.Services;
using Slithin.Core.Sync;
using Slithin.Core.Sync.Repositorys;
using Slithin.Messages;

namespace Slithin.Core.Commands
{
    public class SynchronizeCommand : ICommand
    {
        private readonly SshClient _client;
        private readonly EventStorage _eventStorage;
        private readonly LocalRepository _localRepository;
        private readonly IMailboxService _mailboxService;
        private readonly IPathManager _pathManager;

        public SynchronizeCommand(EventStorage eventStorage,
                                  IMailboxService mailboxService,
                                  LocalRepository localRepository,
                                  IPathManager pathManager,
                                  SshClient client)
        {
            _eventStorage = eventStorage;
            _mailboxService = mailboxService;
            _localRepository = localRepository;
            _pathManager = pathManager;
            _client = client;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return ServiceLocator.SyncService.SyncQueue.Any();
        }

        public void Execute(object parameter)
        {
            _eventStorage.Invoke("beforeSync", new[] { ServiceLocator.SyncService.PersistentSyncQueue.FindAll() });

            if (!_localRepository.GetTemplates().Any() && !Directory.GetFiles(_pathManager.TemplatesDir).Any())
            {
                _mailboxService.Post(new InitStorageMessage());
            }

            NotificationService.Show("Syncing ...");

            _mailboxService.Post(new DownloadNotebooksMessage());

            SyncDeviceDeletions();

            foreach (var item in ServiceLocator.SyncService.PersistentSyncQueue.FindAll())
            {
                _mailboxService.Post(new SyncMessage { Item = item }); // redirect sync job to mailbox for asynchronity
            }

            ServiceLocator.SyncService.PersistentSyncQueue.AnalyseAndAppend();

            ServiceLocator.SyncService.PersistentSyncQueue.DeleteAll();
            ServiceLocator.SyncService.SyncQueue.Clear();

            _eventStorage.Invoke("afterSync");
        }

        public void RaiseExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        private void SyncDeviceDeletions()
        {
            var deviceFiles = _client.RunCommand("ls -p " + PathList.Documents).Result
                .Split('\n', StringSplitOptions.RemoveEmptyEntries).Where(_ => _.EndsWith(".metadata"));
            var localFiles = Directory.GetFiles(_pathManager.NotebooksDir).Where(_ => _.EndsWith(".metadata")).
                Select(_ => Path.GetFileName(_));

            var deltaLocalFiles = localFiles.Except(deviceFiles);

            foreach (var file in deltaLocalFiles)
            {
                var item = new SyncItem
                {
                    Data = JsonConvert.DeserializeObject<Metadata>(File.ReadAllText(Path.Combine(_pathManager.NotebooksDir, file))),
                    Direction = SyncDirection.ToLocal,
                    Action = SyncAction.Remove
                };

                ((Metadata)item.Data).ID = Path.GetFileNameWithoutExtension(file);

                _mailboxService.Post(new SyncMessage { Item = item });
            }
        }
    }
}
