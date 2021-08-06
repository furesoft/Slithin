using System.IO;
using System.Windows.Input;
using Newtonsoft.Json;
using Slithin.Controls;
using Slithin.Core;
using Slithin.Core.Commands;
using Slithin.Core.Remarkable;
using Slithin.Core.Services;
using Slithin.Core.Sync;
using Slithin.UI.Modals;

namespace Slithin.ViewModels
{
    public class NotebooksPageViewModel : BaseViewModel
    {
        private bool _isMoving;
        private Metadata _movingNotebook;
        private Metadata _selectedNotebook;

        public NotebooksPageViewModel(IPathManager _pathManager)
        {
            MakeFolderCommand = DialogService.CreateOpenCommand<MakeFolderModal>(new MakeFolderModalViewModel(_pathManager));
            RemoveNotebookCommand = ServiceLocator.Container.Resolve<RemoveNotebookCommand>();
            MoveCommand = new DelegateCommand(_ =>
            {
                IsMoving = true;
                _movingNotebook = SelectedNotebook;
            }, (_) => SelectedNotebook != null && !IsMoving);

            MoveCancelCommand = new DelegateCommand(_ =>
             {
                 IsMoving = false;
             });

            foreach (var md in Directory.GetFiles(_pathManager.NotebooksDir, "*.metadata", SearchOption.AllDirectories))
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

            MoveHereCommand = new DelegateCommand(_ =>
            {
                MetadataStorage.Local.Move(_movingNotebook, SyncService.NotebooksFilter.Folder);
                IsMoving = false;

                var item = new SyncItem
                {
                    Direction = SyncDirection.ToDevice,
                    Data = MetadataStorage.Local.Get(_movingNotebook.ID),
                    Type = SyncType.Notebook,
                    Action = SyncAction.Update
                };

                SyncService.SyncQueue.Insert(item);

                SyncService.NotebooksFilter.Documents.Clear();
                foreach (var md in MetadataStorage.Local.GetByParent(SyncService.NotebooksFilter.Folder))
                {
                    SyncService.NotebooksFilter.Documents.Add(md);
                }

                SyncService.NotebooksFilter.Documents.Add(new Metadata { Type = "CollectionType", VisibleName = "Up .." });

                SyncService.NotebooksFilter.SortByFolder();
            });
        }

        public bool IsMoving
        {
            get { return _isMoving; }
            set { SetValue(ref _isMoving, value); }
        }

        public ICommand MakeFolderCommand { get; set; }

        public ICommand MoveCancelCommand { get; set; }
        public ICommand MoveCommand { get; set; }
        public ICommand MoveHereCommand { get; set; }
        public ICommand RemoveNotebookCommand { get; set; }

        public Metadata SelectedNotebook
        {
            get { return _selectedNotebook; }
            set { SetValue(ref _selectedNotebook, value); }
        }
    }
}
