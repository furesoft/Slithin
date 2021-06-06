using System.Text.Json.Serialization;

namespace Slithin.Core.Remarkable
{
    public class Metadata
    {
        [JsonPropertyName("deleted")]
        public bool Deleted { get; set; }

        [JsonPropertyName("parent")]
        public string? Parent { get; set; }

        [JsonPropertyName("visibleName")]
        public string? VisibleName { get; set; }

        [JsonPropertyName("version")]
        public int Version { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonConstructor]
        public Metadata()
        {

        }
    }

    public enum MetadataType
    {
        DocumentType,
        CollectionType,
    }
}