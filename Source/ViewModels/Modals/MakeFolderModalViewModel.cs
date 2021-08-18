using System;
using System.Windows.Input;
using ReactiveUI.Fody.Helpers;
using Slithin.Controls;
using Slithin.Core;
using Slithin.Core.Remarkable;
using Slithin.Core.Services;
using Slithin.Core.Sync;

namespace Slithin.ViewModels.Modals
{
    public class MakeFolderModalViewModel : BaseViewModel
    {
        private readonly SynchronisationService _synchronisationService;

        public MakeFolderModalViewModel(SynchronisationService synchronisationService)
        {
            MakeFolderCommand = new DelegateCommand(MakeFolder);
            _synchronisationService = synchronisationService;
        }

        public ICommand MakeFolderCommand { get; set; }

        [Reactive]
        public string Name { get; set; }

        private void MakeFolder(object obj)
        {
            if (!string.IsNullOrEmpty(Name))
            {
                var id = Guid.NewGuid().ToString().ToLower();

                var md = new Metadata
                {
                    ID = id,
                    Parent = _synchronisationService.NotebooksFilter.Folder,
                    Type = "CollectionType",
                    VisibleName = Name
                };

                MetadataStorage.Local.Add(md, out var alreadyAdded);

                if (!alreadyAdded)
                {
                    md.Save();

                    _synchronisationService.NotebooksFilter.Documents.Add(md);
                    _synchronisationService.NotebooksFilter.SortByFolder();

                    var syncItem = new SyncItem
                    {
                        Action = SyncAction.Add,
                        Data = md,
                        Direction = SyncDirection.ToDevice,
                        Type = SyncType.Notebook
                    };

                    _synchronisationService.SyncQueue.Insert(syncItem);

                    DialogService.Close();
                }
                else
                {
                    DialogService.OpenError($"'{md.VisibleName}' already exists");
                }
            }
            else
            {
                //ToDo: show error
            }
        }
    }
}
