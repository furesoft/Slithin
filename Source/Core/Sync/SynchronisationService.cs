using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Newtonsoft.Json;
using Slithin.Core.Remarkable;

namespace Slithin.Core
{
    public class SynchronisationService : INotifyPropertyChanged
    {
        public ObservableCollection<string> Documents { get; set; }

        public ICommand SynchronizeCommand { get; set; }
        public ObservableCollection<Template> Templates { get; set; }

        public ObservableCollection<string> Categories { get; set; }

        private string _selectedCategory;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string SelectedCategory
        {
            get { return _selectedCategory; }
            set { SetValue(ref _selectedCategory, value); RefreshTemplates(); }
        }

        private bool _landscape;
        public bool Landscape
        {
            get { return _landscape; }
            set { SetValue(ref _landscape, value); RefreshTemplates(); }
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

        protected void SetValue<T>(ref T field, T value, [CallerMemberName] string? property = null)
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {

        }

        public SynchronisationService()
        {
            SynchronizeCommand = new DelegateCommand(Synchronize);
            Documents = new();
            Templates = new();
            Categories = new();

            Categories.Add("All");

            PropertyChanged += OnPropertyChanged;
        }

        private void Synchronize(object? obj)
        {
            LoadDocumentMetadata();
            LoadTemplates();

            SelectedCategory = "Grids";
        }

        private void LoadTemplates()
        {
            Templates.Clear();

            TemplateStorage.Instance?.Load();

            var tempCats = TemplateStorage.Instance?.Templates.Select(_ => _.Categories);
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

        private void LoadDocumentMetadata()
        {
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
        }
    }
}