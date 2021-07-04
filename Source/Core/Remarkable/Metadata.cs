using Newtonsoft.Json;

namespace Slithin.Core.Remarkable
{
    public enum MetadataType
    {
        DocumentType,
        CollectionType,
    }

    public class Metadata
    {
        [JsonProperty("deleted")]
        public bool Deleted { get; set; }

        [JsonProperty("parent")]
        public string? Parent { get; set; }

        [JsonProperty("type")]
        public MetadataType Type { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("visibleName")]
        public string? VisibleName { get; set; }
    }
}
