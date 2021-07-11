namespace Slithin.Core.Sync
{
    public class SyncItem
    {
        public SyncAction Action { get; set; }
        public object Data { get; set; }
        public SyncDirection Direction { get; set; }
        public SyncType Type { get; set; }
    }
}
