using System.IO;
using System.Windows.Input;
using Newtonsoft.Json;
using Slithin.Controls;
using Slithin.Core;
using Slithin.Core.Commands;
using Slithin.Core.Remarkable;
using Slithin.UI.Modals;

namespace Slithin.ViewModels
{
    public class NotebooksPageViewModel : BaseViewModel
    {
        private Metadata _selectedNotebook;

        public NotebooksPageViewModel()
        {
            MakeFolderCommand = DialogService.CreateOpenCommand<MakeFolderModal>(new MakeFolderModalViewModel());
            RemoveNotebookCommand = new RemoveNotebookCommand(this);

            foreach (var md in Directory.GetFiles(ServiceLocator.NotebooksDir, "*.metadata", SearchOption.AllDirectories))
            {
                var mdObj = JsonConvert.DeserializeObject<Metadata>(File.ReadAllText(md));
                mdObj.ID = Path.GetFileNameWithoutExtension(md);

                if (File.Exists(Path.ChangeExtension(md, ".content")))
                {
                    mdObj.Content = JsonConvert.DeserializeObject<ContentFile>(File.ReadAllText(Path.ChangeExtension(md, ".content")));
                }

                MetadataStorage.Add(mdObj, out var alreadyAdded);
            }

            foreach (var md in MetadataStorage.GetByParent(""))
            {
                SyncService.NotebooksFilter.Documents.Add(md);
            }

            SyncService.NotebooksFilter.SortByFolder();
        }

        public ICommand MakeFolderCommand { get; set; }

        public ICommand RemoveNotebookCommand { get; set; }

        public Metadata SelectedNotebook
        {
            get { return _selectedNotebook; }
            set { SetValue(ref _selectedNotebook, value); }
        }
    }
}
