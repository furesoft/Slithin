using Newtonsoft.Json;

namespace Slithin.Entities.Remarkable;

public struct ContentPage
{
    [JsonProperty("id")]
    public string ID { get; set; }

    [JsonProperty("template")]
    public ContentTemplate Template { get; set; }
}

public struct ContentTemplate
{
    [JsonProperty("value")]
    public string Value { get; set; }
}

public struct ContentFile : IEqualityComparer<ContentFile>
{
    [JsonProperty("coverPageNumber")] public int CoverPageNumber { get; set; }

    [JsonProperty("extraMetadata")] public object ExtraMetadata { get; set; }

    [JsonProperty("fileType")] public string FileType { get; set; }

    [JsonProperty("pageCount")] public int PageCount { get; set; }

    [JsonProperty("pages")] public ContentPage[] Pages { get; set; }

    public bool Equals(ContentFile x, ContentFile y)
    {
        return x.CoverPageNumber == y.CoverPageNumber
               && x.FileType == y.FileType
               && x.Pages == y.Pages;
    }

    public int GetHashCode(ContentFile obj)
    {
        return HashCode.Combine(obj.Pages, obj.FileType, obj.CoverPageNumber);
    }
}
