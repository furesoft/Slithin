using LiteDB;
using Slithin.Core.Sync;

namespace Slithin.Core
{
    public abstract class BaseViewModel : ReactiveObject
    {
        public bool IsLoaded { get; set; }

        [BsonIgnore]
        public SynchronisationService SyncService => ServiceLocator.SyncService;

        public void Load()
        {
            if (!IsLoaded)
            {
                OnLoad();
            }
        }

        public virtual void OnLoad()
        {
            IsLoaded = true;
        }
    }
}
