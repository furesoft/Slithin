namespace Slithin.Core.Remarkable.Cloud
{
    public class ListItemResult
    {
        public string BlobURLGet { get; set; }
        public string BlobURLGetExpires { get; set; }
        public bool Bookmarked { get; set; }
        public int CurrentPage { get; set; }
        public string ID { get; set; }
        public string Message { get; set; }
        public string ModifiedClient { get; set; }
        public string Parent { get; set; }
        public bool Success { get; set; }
        public string Type { get; set; }
        public int Version { get; set; }
        public string VissibleName { get; set; }
    }
}
