using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using LiteDB;
using Newtonsoft.Json;
using Slithin.Core.Remarkable;
using Slithin.Core.Scripting;
using Slithin.Core.Services;
using Slithin.Core.Sync.Repositorys;
using Slithin.Messages;

namespace Slithin.Core.Sync
{
    public class SynchronisationService : INotifyPropertyChanged
    {
        private readonly LocalRepository _localRepository;
        private readonly IPathManager _pathManager;

        public SynchronisationService(IPathManager pathManager, LiteDatabase db, LocalRepository localRepository)
        {
            TemplateFilter = new();
            NotebooksFilter = new();

            PropertyChanged += OnPropertyChanged;
            SynchronizeCommand = new DelegateCommand(Synchronize);
            SyncQueue = db.GetCollection<SyncItem>();
            _pathManager = pathManager;
            _localRepository = localRepository;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<CustomScreen> CustomScreens { get; set; } = new();

        public NotebooksFilter NotebooksFilter { get; set; }
        public ICommand SynchronizeCommand { get; set; }
        public ILiteCollection<SyncItem> SyncQueue { get; set; }
        public TemplateFilter TemplateFilter { get; set; }
        public ToolsFilter ToolsFilter { get; set; }

        public void LoadFromLocal()
        {
            LoadTemplates();

            TemplateFilter.SelectedCategory = "All";
        }

        public void LoadTemplates()
        {
            TemplateFilter.Templates.Clear();

            // Load local Templates
            TemplateStorage.Instance?.Load();

            // Load Category Names
            var tempCats = TemplateStorage.Instance?.Templates.Select(_ => _.Categories);
            TemplateFilter.Categories.Add("All");

            foreach (var item in tempCats)
            {
                foreach (var cat in item)
                {
                    if (!TemplateFilter.Categories.Contains(cat))
                    {
                        TemplateFilter.Categories.Add(cat);
                    }
                }
            }

            foreach (var item in TemplateStorage.Instance?.Templates)
            {
                TemplateFilter.Templates.Add(item);
            }
        }

        protected void SetValue<T>(ref T field, T value, [CallerMemberName] string property = null)
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        private void SyncDeviceDeletions()
        {
            var mailboxService = ServiceLocator.Container.Resolve<IMailboxService>();

            var deviceFiles = ServiceLocator.Client.RunCommand("ls -p " + PathList.Documents).Result
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

            mailboxService.Post(new ShowStatusMessage { Message = "Syncing ..." });

            SyncDeviceDeletions();

            foreach (var item in SyncQueue.FindAll())
            {
                mailboxService.Post(new SyncMessage { Item = item }); // redirect sync job to mailbox for asynchronity
            }

            mailboxService.Post(new DownloadNotebooksMessage());

            SyncQueue.AnalyseAndAppend();

            SyncQueue.DeleteAll();

            events.Invoke("afterSync");
        }
    }
}
