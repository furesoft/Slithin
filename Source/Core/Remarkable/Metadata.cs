using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Slithin.Core.Remarkable
{
    public enum MetadataType
    {
        DocumentType,
        CollectionType,
    }

    public class ContentFile
    {
        [JsonProperty("coverPageNumber")]
        public int CoverPageNumber { get; set; }

        [JsonProperty("fileType")]
        public string FileType { get; set; }

        [JsonProperty("pages")]
        public string[] Pages { get; set; }
    }

    public class Metadata
    {
        [JsonIgnore]
        public ContentFile Content { get; set; }

        [JsonProperty("deleted")]
        public bool Deleted { get; set; }

        [JsonIgnore]
        public string ID { get; internal set; }

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
