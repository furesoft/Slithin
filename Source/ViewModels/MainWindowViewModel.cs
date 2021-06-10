using System.Collections.ObjectModel;
using System.Windows.Input;
using Newtonsoft.Json;
using Slithin.Core;
using Slithin.Core.Remarkable;

namespace Slithin.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public ICommand LoadMetadataCommand { get; set; }

        public ObservableCollection<string> Documents { get; set; }

        public ICommand RefreshCommand { get; set; }

        public MainWindowViewModel()
        {
            LoadMetadataCommand = new DelegateCommand(LoadMetadata);
            Documents = new();
            RefreshCommand = new DelegateCommand(Refresh);
        }

        private void Refresh(object? obj)
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

        private void LoadMetadata(object? obj)
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
