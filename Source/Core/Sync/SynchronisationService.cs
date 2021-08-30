using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using LiteDB;
using Newtonsoft.Json;
using Renci.SshNet;
using Slithin.Core.Remarkable;
using Slithin.Core.Scripting;
using Slithin.Core.Services;
using Slithin.Core.Sync.Repositorys;
using Slithin.Messages;
using Slithin.Models;

namespace Slithin.Core.Sync
{
    public class SynchronisationService : ReactiveObject
    {
        private readonly SshClient _client;
        private readonly LocalRepository _localRepository;
        private readonly IPathManager _pathManager;

        public SynchronisationService(IPathManager pathManager,
                                      LiteDatabase db,
                                      LocalRepository localRepository,
                                      SshClient client)
        {
            TemplateFilter = new();
            NotebooksFilter = new();

            SynchronizeCommand = new DelegateCommand(Synchronize);
            SyncQueue = db.GetCollection<SyncItem>();
            _pathManager = pathManager;
            _localRepository = localRepository;
            _client = client;
        }

        public ObservableCollection<CustomScreen> CustomScreens { get; set; } = new();

        public NotebooksFilter NotebooksFilter { get; set; }
        public ICommand SynchronizeCommand { get; set; }
        public ILiteCollection<SyncItem> SyncQueue { get; set; }
        public TemplateFilter TemplateFilter { get; set; }
        public ToolsFilter ToolsFilter { get; set; }

        private void SyncDeviceDeletions()
        {
            var mailboxService = ServiceLocator.Container.Resolve<IMailboxService>();

            var deviceFiles = _client.RunCommand("ls -p " + PathList.Documents).Result
                .Split('\n', System.StringSplitOptions.RemoveEmptyEntries).Where(_ => _.EndsWith(".metadata"));
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

                mailboxService.Post(new SyncMessage { Item = item });
            }
        }

        private void Synchronize(object obj)
        {
            var events = ServiceLocator.Container.Resolve<EventStorage>();
            var mailboxService = ServiceLocator.Container.Resolve<IMailboxService>();

            events.Invoke("beforeSync", new[] { SyncQueue.FindAll() });

            if (!_localRepository.GetTemplates().Any() && !Directory.GetFiles(_pathManager.TemplatesDir).Any())
            {
                mailboxService.Post(new InitStorageMessage());
            }

            NotificationService.Show("Syncing ...");

            mailboxService.Post(new DownloadNotebooksMessage());

            SyncDeviceDeletions();

            foreach (var item in SyncQueue.FindAll())
            {
                mailboxService.Post(new SyncMessage { Item = item }); // redirect sync job to mailbox for asynchronity
            }

            SyncQueue.AnalyseAndAppend();

            SyncQueue.DeleteAll();

            events.Invoke("afterSync");
        }
    }
}
