using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Slithin.Core.Remarkable.Models;
using Slithin.Core.Remarkable;

namespace Slithin.Core.Remarkable.Models;

public struct ContentFile : IEqualityComparer<ContentFile>
{
    [JsonProperty("coverPageNumber")] public int CoverPageNumber { get; set; }

    [JsonProperty("extraMetadata")] public object ExtraMetadata { get; set; }

    [JsonProperty("fileType")] public string FileType { get; set; }

    [JsonProperty("pageCount")] public int PageCount { get; set; }

    [JsonProperty("pages")] public string[] Pages { get; set; }

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
