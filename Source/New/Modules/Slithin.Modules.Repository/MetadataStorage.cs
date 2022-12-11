namespace Slithin.Entities;

public class MetadataStorage
{
    public static MetadataStorage Local = new();

    private readonly Dictionary<string, Metadata> _storage = new();

    /*
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

    public void AddMetadata(Metadata metadata, out bool alreadyAdded)
    {
        if (_storage.ContainsKey(metadata.ID))
        {
            alreadyAdded = true;
            return;
        }

        _storage.Add(metadata.ID, metadata);
        alreadyAdded = false;
    }

    public void Save(Metadata metadata)
    {
        var pathManager = ServiceLocator.Container.Resolve<IPathManager>();

        File.WriteAllText(Path.Combine(pathManager.NotebooksDir, metadata.ID + ".metadata"),
            JsonConvert.SerializeObject(this, Formatting.Indented));

        File.WriteAllText(Path.Combine(pathManager.NotebooksDir, metadata.ID + ".content"),
            JsonConvert.SerializeObject(metadata.Content, Formatting.Indented));
    }
    */

    public void Clear()
    {
        _storage.Clear();
    }

    public IEnumerable<Metadata> GetAll()
    {
        return _storage.Values;
    }

    public IEnumerable<Metadata> GetByParent(string parent)
    {
        var list = new List<Metadata>();

        foreach (var item in _storage)
        {
            if (item.Value.Parent is not null && item.Value.Parent.Equals(parent))
            {
                list.Add(item.Value);
            }
        }

        return list;
    }

    public Metadata GetMetadata(string id)
    {
        return _storage[id];
    }

    public IEnumerable<string> GetNames()
    {
        return _storage.Keys;
    }

    /*
    public void Move(Metadata md, string folder)
    {
        md.Parent = folder;
        md.Version++;

        _storage[md.ID] = md; //replace metadata with changed md

        md.Save();

        md.Upload();

        var xochitl = ServiceLocator.Container.Resolve<IRemarkableDevice>();
        xochitl.ReloadDevice();
    }

    public void Upload(bool onlyMetadata = false)
    {
        var scp = ServiceLocator.Container.Resolve<IRemarkableDevice>();
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
    */

    public void Remove(Metadata tmpl)
    {
        _storage.Remove(tmpl.ID);
    }
}
