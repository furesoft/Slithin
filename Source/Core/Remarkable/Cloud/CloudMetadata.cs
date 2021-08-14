using Slithin.Core.Remarkable;
namespace Slithin.Core.Remarkable.Cloud
{
    public class CloudMetadata
    {
        public bool Deleted { get; set; }

        public string ID { get; internal set; }

        public int LastOpenedPage { get; set; }
        public bool MetadataModified { get; set; }
        public bool Modified { get; set; }
        public string ModifiedClient { get; set; }
        public string Parent { get; set; }

        public bool Pinned { get; set; }

        public string Type { get; set; }

        public int Version { get; set; }

        public string VissibleName { get; set; }
    }
}
