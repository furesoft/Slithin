using System.Collections.Generic;

namespace Slithin.Core.Remarkable;

public class MetadataStorage
{
    public static MetadataStorage Local = new();

    private readonly Dictionary<string, Metadata> _storage = new();

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

    public void Move(Metadata md, string folder)
    {
        md.Parent = folder;
        md.Version++;

        _storage[md.ID] = md; //replace metadata with changed md

        md.Save();

        md.Upload();

        var xochitl = ServiceLocator.Container.Resolve<Xochitl>();
        xochitl.ReloadDevice();
    }

    public void Remove(Metadata tmpl)
    {
        _storage.Remove(tmpl.ID);
    }
}
