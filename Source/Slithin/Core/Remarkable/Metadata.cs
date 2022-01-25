using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Newtonsoft.Json;
using Slithin.Core.Services;

namespace Slithin.Core.Remarkable;

public class Metadata : NotifyObject, IEqualityComparer<Metadata>
{
    private string _visibleName;

    [JsonIgnore] public ContentFile Content { get; set; }

    [JsonProperty("deleted")] public bool Deleted { get; set; }

    [JsonIgnore] public string ID { get; internal set; }

    [JsonProperty("lastOpenedPage")] public int LastOpenedPage { get; set; }

    [JsonIgnore] public PageData PageData { get; set; }

    [JsonProperty("parent")] public string Parent { get; set; }

    [JsonProperty("pinned")] public bool Pinned { get; set; }

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
        return x!.Content.Equals(y.Content)
               && x.Deleted == y.Deleted
               && x.ID.Equals(y.ID)
               && x.LastOpenedPage == y.LastOpenedPage
               && x.Parent.Equals(y.Parent)
               && x.Type.Equals(y.Type)
               && x.Version == y.Version
               && x.VisibleName.Equals(y.VisibleName);
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

    public static Metadata Load(string id)
    {
        var pathManager = ServiceLocator.Container.Resolve<IPathManager>();

        var mdObj = JsonConvert.DeserializeObject<Metadata>(
            File.ReadAllText(Path.Combine(pathManager.NotebooksDir, id + ".metadata")));
        mdObj!.ID = id;

        if (File.Exists(Path.Combine(pathManager.NotebooksDir, id + ".content")))
        {
            mdObj.Content = JsonConvert.DeserializeObject<ContentFile>(
                File.ReadAllText(Path.Combine(pathManager.NotebooksDir, id + ".content")));
        }

        if (File.Exists(Path.Combine(pathManager.NotebooksDir, id + ".pagedata")))
        {
            mdObj.PageData.Parse(File.ReadAllText(Path.Combine(pathManager.NotebooksDir, id + ".pagedata")));
        }
        else
        {
            var data = new[] { "Blank" };
            var pg = new PageData { Data = data };

            mdObj.PageData = pg;
        }

        return mdObj;
    }

    public void Save()
    {
        var pathManager = ServiceLocator.Container.Resolve<IPathManager>();

        File.WriteAllText(Path.Combine(pathManager.NotebooksDir, ID + ".metadata"),
            JsonConvert.SerializeObject(this, Formatting.Indented));

        //if (new ContentFile?(Content).HasValue) //Das kann niemals false sein!
        {
            File.WriteAllText(Path.Combine(pathManager.NotebooksDir, ID + ".content"),
                JsonConvert.SerializeObject(Content, Formatting.Indented));
        }
    }

    public override string ToString()
    {
        return VisibleName;
    }
}
