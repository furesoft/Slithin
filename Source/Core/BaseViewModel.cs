using LiteDB;
using Slithin.Core.Sync;

namespace Slithin.Core
{
    public abstract class BaseViewModel : ReactiveObject
    {
        [BsonIgnore]
        public SynchronisationService SyncService => ServiceLocator.SyncService;
    }
}
