using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using LiteDB;
using Slithin.Core.Remarkable;
using Slithin.Core.Sync;
using Slithin.Messages;

namespace Slithin.Core
{
    public class SynchronisationService : INotifyPropertyChanged
    {
        public SynchronisationService()
        {
            Documents = new();
            TemplateFilter = new();

            PropertyChanged += OnPropertyChanged;
            SynchronizeCommand = new DelegateCommand(Synchronize);
            SyncQueue = ServiceLocator.Database.GetCollection<SyncItem>();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<string> Documents { get; set; }

        public bool IsSyncNeeded => !Directory.Exists(ServiceLocator.TemplatesDir);

        public ICommand SynchronizeCommand { get; set; }
        public ILiteCollection<SyncItem> SyncQueue { get; set; }

        public TemplateFilter TemplateFilter { get; set; }

        public void LoadFromLocal()
        {
            LoadTemplates();

            TemplateFilter.SelectedCategory = "All";
        }

        protected void SetValue<T>(ref T field, T value, [CallerMemberName] string? property = null)
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void LoadDocumentMetadata()
        {
            /*
            var allDocumentsStream = ServiceLocator.Client.RunCommand("ls " + PathList.Documents).Result;
            var filenames = allDocumentsStream.Split('\n');

            foreach (var filename in filenames)
            {
                if (filename.EndsWith(".metadata"))
                {
                    var filecontent = ServiceLocator.Client.RunCommand("cat " + PathList.Documents + "/" + filename);
                    var metadata = JsonConvert.DeserializeObject<Metadata>(filecontent.Result);
                    MetadataStorage.Add(metadata);

                    Documents.Add(metadata.VisibleName);
                }
            }
            */
        }

        private void LoadTemplates()
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

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
        }

        private void Synchronize(object? obj)
        {
            ServiceLocator.Mailbox.Post(new ShowStatusMessage { Message = "Syncing ..." });

            foreach (var item in SyncQueue.FindAll())
            {
                ServiceLocator.Mailbox.Post(new SyncMessage { Item = item }); // redirect sync job to mailbox for asynchronity
            }

            SyncQueue.AnalyseAndAppend();

            SyncQueue.DeleteAll();
        }
    }
}
