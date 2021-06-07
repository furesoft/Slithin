using System.Text.Json.Serialization;
using LiteDB;

namespace Slithin.Core.Remarkable
{
    public class Template
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("filename")]
        public string? Filename { get; set; }

        [JsonPropertyName("iconCode")]
        public string? IconCode { get; set; }

        [JsonPropertyName("categories")]
        public string[]? Categories { get; set; }

        [JsonPropertyName("landscape")]
        public bool Landscape { get; set; }
    }
}