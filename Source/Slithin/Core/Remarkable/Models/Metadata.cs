using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Newtonsoft.Json;
using Slithin.Core.MVVM;
using Slithin.Core.Services;

namespace Slithin.Core.Remarkable.Models;

public class Metadata : NotifyObject, IEqualityComparer<Metadata>
{
    private bool _pinned;
    private string _visibleName;

    [JsonIgnore] public ContentFile Content { get; set; }

    [JsonProperty("deleted")] public bool Deleted { get; set; }

    [JsonIgnore] public string ID { get; internal set; }

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

    public void Save()
    {
        var pathManager = ServiceLocator.Container.Resolve<IPathManager>();

        File.WriteAllText(Path.Combine(pathManager.NotebooksDir, ID + ".metadata"),
            JsonConvert.SerializeObject(this, Formatting.Indented));

        File.WriteAllText(Path.Combine(pathManager.NotebooksDir, ID + ".content"),
            JsonConvert.SerializeObject(Content, Formatting.Indented));
    }

    public override string ToString()
    {
        return VisibleName;
    }

    public void Upload(bool onlyMetadata = false)
    {
        var scp = ServiceLocator.Container.Resolve<ISSHService>();
        var notebooksDir = ServiceLocator.Container.Resolve<IPathManager>().NotebooksDir;

        scp.Upload(new FileInfo(Path.Combine(notebooksDir, ID + ".metadata")),
                                PathList.Documents + "/" + ID + ".metadata");

        if (Type == "DocumentType" &&
                                (Content.FileType == "pdf" || Content.FileType == "epub") && !onlyMetadata)
        {
            scp.Upload(new FileInfo(Path.Combine(notebooksDir, ID + "." + Content.FileType)),
                PathList.Documents + "/" + ID + "." + Content.FileType);
            scp.Upload(new FileInfo(Path.Combine(notebooksDir, ID + ".content")),
                PathList.Documents + "/" + ID + ".content");
        }
    }
}
