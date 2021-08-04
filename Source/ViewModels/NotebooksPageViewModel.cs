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
        private bool _isMoving;
        private Metadata _selectedNotebook;

        public NotebooksPageViewModel()
        {
            MakeFolderCommand = DialogService.CreateOpenCommand<MakeFolderModal>(new MakeFolderModalViewModel());
            RemoveNotebookCommand = new RemoveNotebookCommand(this);
            MoveCommand = new DelegateCommand(_ =>
            {
                IsMoving = true;
            }, (_) => SelectedNotebook != null && !IsMoving);

            MoveCancelCommand = new DelegateCommand(_ =>
             {
                 IsMoving = false;
             });

            foreach (var md in Directory.GetFiles(ServiceLocator.NotebooksDir, "*.metadata", SearchOption.AllDirectories))
            {
                var mdObj = JsonConvert.DeserializeObject<Metadata>(File.ReadAllText(md));
                mdObj.ID = Path.GetFileNameWithoutExtension(md);

                if (File.Exists(Path.ChangeExtension(md, ".content")))
                {
                    mdObj.Content = JsonConvert.DeserializeObject<ContentFile>(File.ReadAllText(Path.ChangeExtension(md, ".content")));
                }
                if (File.Exists(Path.ChangeExtension(md, ".pagedata")))
                {
                    mdObj.PageData.Parse(File.ReadAllText(Path.ChangeExtension(md, ".pagedata")));
                }

                MetadataStorage.Local.Add(mdObj, out var alreadyAdded);
            }

            foreach (var md in MetadataStorage.Local.GetByParent(""))
            {
                SyncService.NotebooksFilter.Documents.Add(md);
            }

            SyncService.NotebooksFilter.SortByFolder();
        }

        public bool IsMoving
        {
            get { return _isMoving; }
            set { SetValue(ref _isMoving, value); }
        }

        public ICommand MakeFolderCommand { get; set; }

        public ICommand MoveCancelCommand { get; set; }
        public ICommand MoveCommand { get; set; }
        public ICommand RemoveNotebookCommand { get; set; }

        public Metadata SelectedNotebook
        {
            get { return _selectedNotebook; }
            set { SetValue(ref _selectedNotebook, value); }
        }
    }
}
