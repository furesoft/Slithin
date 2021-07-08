using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Newtonsoft.Json;
using Slithin.Core.Remarkable;
using Slithin.Core.Sync;
using Slithin.Messages;

namespace Slithin.Core
{
    public class SynchronisationService : INotifyPropertyChanged
    {
        private bool _landscape;
        private string _selectedCategory;

        public SynchronisationService()
        {
            Documents = new();
            Templates = new();
            Categories = new();

            Categories.Add("All");

            PropertyChanged += OnPropertyChanged;
            SynchronizeCommand = new DelegateCommand(Synchronize);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<string> Categories { get; set; }
        public ObservableCollection<string> Documents { get; set; }

        public bool IsSyncNeeded => !Directory.Exists(ServiceLocator.TemplatesDir);

        public bool Landscape
        {
            get { return _landscape; }
            set { SetValue(ref _landscape, value); RefreshTemplates(); }
        }

        public string SelectedCategory
        {
            get { return _selectedCategory; }
            set { SetValue(ref _selectedCategory, value); RefreshTemplates(); }
        }

        public ICommand SynchronizeCommand { get; set; }
        public List<SyncItem> SyncQueue { get; set; } = new();
        public ObservableCollection<Template> Templates { get; set; }

        public void LoadFromLocal()
        {
            LoadTemplates();

            SelectedCategory = "All";
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
            Templates.Clear();

            if (ServiceLocator.SyncService.IsSyncNeeded)
            {
                ServiceLocator.Device.GetTemplates();
            }

            // var deviceTemplates = ServiceLocator.Device.GetTemplates();
            // Load local Templates
            TemplateStorage.Instance?.Load();

            // Load Category Names
            var tempCats = TemplateStorage.Instance?.Templates.Select(_ => _.Categories);
            Categories.Add("All");

            foreach (var item in tempCats)
            {
                foreach (var cat in item)
                {
                    if (!Categories.Contains(cat))
                    {
                        Categories.Add(cat);
                    }
                }
            }

            foreach (var item in TemplateStorage.Instance?.Templates)
            {
                Templates.Add(item);
            }
        }

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
        }

        private void RefreshTemplates()
        {
            Templates.Clear();

            if (SelectedCategory == "All")
            {
                foreach (var item in TemplateStorage.Instance?.Templates.Where(_ => Landscape == _.Landscape))
                {
                    Templates.Add(item);
                }
            }
            else
            {
                foreach (var item in TemplateStorage.Instance?.Templates.Where(_ => _.Categories.Contains(SelectedCategory) && Landscape == _.Landscape))
                {
                    Templates.Add(item);
                }
            }
        }

        private void Synchronize(object? obj)
        {
            // SelectedCategory = "All";

            foreach (var item in SyncQueue)
            {
                ServiceLocator.Mailbox.Post(new SyncMessage { Item = item }); // redirect sync job to mailbox for asynchronity
            }

            SyncQueue.Clear();
        }
    }
}
