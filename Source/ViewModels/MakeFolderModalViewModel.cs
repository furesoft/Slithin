using System;
using System.IO;
using System.Windows.Input;
using Newtonsoft.Json;
using Slithin.Controls;
using Slithin.Core;
using Slithin.Core.Remarkable;
using Slithin.Core.Sync;

namespace Slithin.ViewModels
{
    public class MakeFolderModalViewModel : BaseViewModel
    {
        private string _name;

        public MakeFolderModalViewModel()
        {
            MakeFolderCommand = new DelegateCommand(MakeFolder);
        }

        public ICommand MakeFolderCommand { get; set; }

        public string Name
        {
            get { return _name; }
            set { SetValue(ref _name, value); }
        }

        private void MakeFolder(object obj)
        {
            var id = Guid.NewGuid();

            var md = new Metadata();
            md.ID = id.ToString();
            md.Parent = ServiceLocator.SyncService.NotebooksFilter.Folder;
            md.Type = "CollectionType";
            md.VisibleName = Name;

            MetadataStorage.Local.Add(md, out var alreadyAdded);

            if (!alreadyAdded)
            {
                var path = Path.Combine(ServiceLocator.NotebooksDir, md.ID + ".metadata");
                var mdContent = JsonConvert.SerializeObject(md);
                File.WriteAllText(path, mdContent);

                ServiceLocator.SyncService.NotebooksFilter.Documents.Add(md);
                ServiceLocator.SyncService.NotebooksFilter.SortByFolder();

                var syncItem = new SyncItem();
                syncItem.Action = SyncAction.Add;
                syncItem.Data = md;
                syncItem.Direction = SyncDirection.ToDevice;
                syncItem.Type = SyncType.Notebook;

                ServiceLocator.SyncService.SyncQueue.Insert(syncItem);

                DialogService.Close();
            }
            else
            {
                DialogService.OpenError($"'{md.VisibleName}' already exists");
            }
        }
    }
}
