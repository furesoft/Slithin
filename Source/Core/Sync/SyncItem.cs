namespace Slithin.Core.Sync
{
    public class SyncItem
    {
        public object Data { get; set; }
        public SyncDirection Direction { get; set; }
        public SyncType Type { get; set; }
    }
}
