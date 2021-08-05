using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using LiteDB;
using Newtonsoft.Json;
using Slithin.Core.Remarkable;
using Slithin.Messages;

namespace Slithin.Core.Sync
{
    public class SynchronisationService : INotifyPropertyChanged
    {
        public SynchronisationService()
        {
            TemplateFilter = new();
            NotebooksFilter = new();

            PropertyChanged += OnPropertyChanged;
            SynchronizeCommand = new DelegateCommand(Synchronize);
            SyncQueue = ServiceLocator.Database.GetCollection<SyncItem>();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<CustomScreen> CustomScreens { get; set; } = new();
        public bool IsSyncNeeded => !Directory.Exists(ServiceLocator.TemplatesDir);
        public NotebooksFilter NotebooksFilter { get; set; }
        public ICommand SynchronizeCommand { get; set; }
        public ILiteCollection<SyncItem> SyncQueue { get; set; }

        public TemplateFilter TemplateFilter { get; set; }

        public void LoadFromLocal()
        {
            LoadTemplates();

            TemplateFilter.SelectedCategory = "All";
        }

        public void LoadTemplates()
        {
            TemplateFilter.Templates.Clear();

            if (ServiceLocator.SyncService.IsSyncNeeded)
            {
                ServiceLocator.Device.GetTemplates();
            }

            // var deviceTemplates = ServiceLocator.Device.GetTemplates();
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

        protected void SetValue<T>(ref T field, T value, [CallerMemberName] string? property = null)
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
        }

        private void SyncDeviceDeletions()
        {
            var deviceFiles = ServiceLocator.Client.RunCommand("ls -p " + PathList.Documents).Result
                .Split('\n', System.StringSplitOptions.RemoveEmptyEntries).Where(_ => _.EndsWith(".metadata"));
            var localFiles = Directory.GetFiles(ServiceLocator.NotebooksDir).Where(_ => _.EndsWith(".metadata")).
                Select(_ => Path.GetFileName(_));

            var deltaLocalFiles = localFiles.Except(deviceFiles);

            foreach (var file in deltaLocalFiles)
            {
                var item = new SyncItem();
                item.Data = JsonConvert.DeserializeObject<Metadata>(File.ReadAllText(Path.Combine(ServiceLocator.NotebooksDir, file)));
                item.Direction = SyncDirection.ToLocal;
                item.Action = SyncAction.Remove;

                ((Metadata)item.Data).ID = Path.GetFileNameWithoutExtension(file);

                ServiceLocator.Mailbox.Post(new SyncMessage { Item = item });
            }
        }

        private void Synchronize(object? obj)
        {
            ServiceLocator.Events.Invoke("beforeSync", new[] { SyncQueue.FindAll() });

            if (!ServiceLocator.Local.GetTemplates().Any() && !Directory.GetFiles(ServiceLocator.TemplatesDir).Any())
            {
                ServiceLocator.Mailbox.Post(new InitStorageMessage());
            }

            ServiceLocator.Mailbox.Post(new ShowStatusMessage { Message = "Syncing ..." });

            SyncDeviceDeletions();

            foreach (var item in SyncQueue.FindAll())
            {
                ServiceLocator.Mailbox.Post(new SyncMessage { Item = item }); // redirect sync job to mailbox for asynchronity
            }

            ServiceLocator.Mailbox.Post(new DownloadNotebooksMessage());

            SyncQueue.AnalyseAndAppend();

            SyncQueue.DeleteAll();

            ServiceLocator.Events.Invoke("afterSync");
        }
    }
}
