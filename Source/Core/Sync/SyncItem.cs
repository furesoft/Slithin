using LiteDB;

namespace Slithin.Core.Sync
{
    public class SyncItem
    {
        public ObjectId _id { get; set; }
        public SyncAction Action { get; set; }
        public object Data { get; set; }
        public SyncDirection Direction { get; set; }
        public SyncType Type { get; set; }
    }
}
