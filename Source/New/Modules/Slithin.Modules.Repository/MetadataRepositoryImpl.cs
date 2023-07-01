using AuroraModularis.Core;
using DotNext;
using Newtonsoft.Json;
using Slithin.Entities.Remarkable;
using Slithin.Modules.BaseServices.Models;
using Slithin.Modules.Device.Models;
using Slithin.Modules.Repository.Models;

namespace Slithin.Modules.Repository;

public class MetadataRepositoryImpl : IMetadataRepository
{
    private readonly Dictionary<string, Metadata> _storage = new();

    public Metadata Load(string id)
    {
        var pathManager = ServiceContainer.Current.Resolve<IPathManager>();

        var mdObj = JsonConvert.DeserializeObject<Metadata>(
            File.ReadAllText(Path.Combine(pathManager.NotebooksDir, $"{id}.metadata")));
        mdObj!.ID = id;

        if (File.Exists(Path.Combine(pathManager.NotebooksDir, $"{id}.content")))
        {
            //  mdObj.Content = JsonConvert.DeserializeObject<ContentFile>(
            //    File.ReadAllText(Path.Combine(pathManager.NotebooksDir, $"{id}.content")));
        }

        if (File.Exists(Path.Combine(pathManager.NotebooksDir, $"{id}.pagedata")))
        {
            mdObj.PageData = PageData.Parse(File.ReadAllText(Path.Combine(pathManager.NotebooksDir, $"{id}.pagedata")));
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

        _storage.TryAdd(metadata.ID, metadata);
        alreadyAdded = false;
    }

    public void SaveToDisk(Metadata metadata)
    {
        var pathManager = ServiceContainer.Current.Resolve<IPathManager>();

        File.WriteAllText(Path.Combine(pathManager.NotebooksDir, $"{metadata.ID}.metadata"),
            JsonConvert.SerializeObject(this, Formatting.Indented));

        File.WriteAllText(Path.Combine(pathManager.NotebooksDir, $"{metadata.ID}.content"),
            JsonConvert.SerializeObject(metadata.Content, Formatting.Indented));
    }

    public void Clear()
    {
        _storage.Clear();
    }

    public IEnumerable<Metadata> GetAll()
    {
        return _storage.Values;
    }

    public Result<Metadata> GetByName(string name)
    {
        return _storage.Values.FirstOrDefault(_ => _.VisibleName == name) ?? new Result<Metadata>(new Exception($"Cannot find notebook with name '{name}'"));
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

    public void Move(Metadata md, string folder)
    {
        md.Parent = folder;
        md.Version++;

        _storage[md.ID] = md; //replace metadata with changed md

        SaveToDisk(md);

        Upload(md);

        var remarkableDevice = ServiceContainer.Current.Resolve<IRemarkableDevice>();
        remarkableDevice.Reload();
    }

    public void Upload(Metadata md, bool onlyMetadata = false)
    {
        var scp = ServiceContainer.Current.Resolve<IRemarkableDevice>();
        var notebooksDir = ServiceContainer.Current.Resolve<IPathManager>().NotebooksDir;
        var pathList = ServiceContainer.Current.Resolve<DevicePathList>();

        scp.Upload(new FileInfo(Path.Combine(notebooksDir, $"{md.ID}.metadata")),
            $"{pathList.Notebooks}{md.ID}.metadata");

        if (md.Type == "DocumentType" &&
                                (md.Content.FileType == "pdf" || md.Content.FileType == "epub") && !onlyMetadata)
        {
            scp.Upload(new FileInfo(Path.Combine(notebooksDir, $"{md.ID}.{md.Content.FileType}")),
                $"{pathList.Notebooks}{md.ID}.{md.Content.FileType}");
            scp.Upload(new FileInfo(Path.Combine(notebooksDir, $"{md.ID}.content")),
                $"{pathList.Notebooks}{md.ID}.content");
        }
    }

    public void Remove(Metadata md)
    {
        _storage.Remove(md.ID);
    }
}
