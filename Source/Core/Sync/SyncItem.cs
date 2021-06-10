namespace Slithin.Core.Sync
{
    public class SyncItem
    {
        public string Name { get; set; }
        public SyncType Type { get; set; }
        public SyncDirection Direction { get; set; }
    }

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
}