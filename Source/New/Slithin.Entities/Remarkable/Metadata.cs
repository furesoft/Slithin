using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Slithin.Core.MVVM;

namespace Slithin.Entities.Remarkable;

public class Metadata : NotifyObject, IEqualityComparer<Metadata>
{
    private bool _pinned;
    private string _visibleName;

    [JsonIgnore] public ContentFile Content { get; set; }

    [JsonProperty("deleted")] public bool Deleted { get; set; }

    [JsonIgnore] public string ID { get; set; }

    [JsonProperty("pinned")]
    public bool IsPinned
    {
        get { return _pinned; }
        set { SetValue(ref _pinned, value); }
    }

    [JsonProperty("lastOpenedPage")] public int LastOpenedPage { get; set; }

    [JsonProperty("modified")] public bool Modified { get; set; }
    [JsonIgnore] public PageData PageData { get; set; }

    [JsonProperty("parent")] public string Parent { get; set; }
    [JsonProperty("type")] public string Type { get; set; }

    [JsonProperty("version")] public int Version { get; set; }

    [JsonProperty("visibleName")]
    public string VisibleName
    {
        get => _visibleName;
        set => SetValue(ref _visibleName, value);
    }

    public bool Equals(Metadata x, Metadata y)
    {
        return x.ID.Equals(y.ID);
    }

    public int GetHashCode([DisallowNull] Metadata obj)
    {
        return HashCode.Combine(obj.VisibleName,
            obj.Version,
            obj.Type,
            obj.Parent,
            obj.LastOpenedPage,
            obj.ID,
            obj.Deleted,
            obj.Content);
    }

    public override string ToString()
    {
        return VisibleName;
    }
}
