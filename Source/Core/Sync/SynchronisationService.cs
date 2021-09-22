using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using LiteDB;
using Newtonsoft.Json;
using Renci.SshNet;
using Slithin.Core.Commands;
using Slithin.Core.Remarkable;
using Slithin.Core.Scripting;
using Slithin.Core.Services;
using Slithin.Core.Sync.Repositorys;
using Slithin.Messages;
using Slithin.Models;

namespace Slithin.Core.Sync
{
    public class SynchronisationService : NotifyObject
    {
        public SynchronisationService(LiteDatabase db)
        {
            TemplateFilter = new();
            NotebooksFilter = new();

            SynchronizeCommand = ServiceLocator.Container.Resolve<SynchronizeCommand>();

            PersistentSyncQueue = db.GetCollection<SyncItem>();

            foreach (var item in PersistentSyncQueue.FindAll())
            {
                SyncQueue.Add(item);
            }
        }

        public ObservableCollection<CustomScreen> CustomScreens { get; set; } = new();

        public NotebooksFilter NotebooksFilter { get; set; }
        public ILiteCollection<SyncItem> PersistentSyncQueue { get; set; }
        public SynchronizeCommand SynchronizeCommand { get; set; }
        public ObservableCollection<SyncItem> SyncQueue { get; set; } = new();
        public TemplateFilter TemplateFilter { get; set; }
        public ToolsFilter ToolsFilter { get; set; }

        public void AddToSyncQueue(SyncItem item)
        {
            SyncQueue.Add(item);
            PersistentSyncQueue.Insert(item);
            SynchronizeCommand.RaiseExecuteChanged();
        }

        public void RemoveFromSyncQueue(SyncItem item)
        {
            SyncQueue.Remove(item);
            PersistentSyncQueue.Delete(item._id);
            SynchronizeCommand.RaiseExecuteChanged();
        }
    }
}
