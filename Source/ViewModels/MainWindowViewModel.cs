using System;
using System.IO;
using System.Windows.Input;
using Slithin.Core;
using Slithin.Core.Remarkable;

namespace Slithin.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public ICommand LoadMetadataCommand { get; set; }

        public MainWindowViewModel()
        {
            LoadMetadataCommand = new DelegateCommand(LoadMetadata);
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
                    var metadata = System.Text.Json.JsonSerializer.Deserialize<Metadata>(filecontent.Result);
                    MetadataStorage.Add(metadata);
                }
            }

        }
    }
}
