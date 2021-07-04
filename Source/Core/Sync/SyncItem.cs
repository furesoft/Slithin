namespace Slithin.Core.Sync
{
    public enum SyncDirection
    {
        ToDevice,
        ToLocal
    }

    public enum SyncType
    {
        Template,
        Screen,
        AdjustTemplateConfig,
    }

    public class SyncItem
    {
        public SyncDirection Direction { get; set; }
        public string Name { get; set; }
        public SyncType Type { get; set; }
    }
}
