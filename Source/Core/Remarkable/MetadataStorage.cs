using System.Collections.Generic;
using Slithin.Core.Services;

namespace Slithin.Core.Remarkable
{
    public class MetadataStorage
    {
        public static MetadataStorage Local = new(ServiceLocator.Container.Resolve<IPathManager>());

        private readonly IPathManager _pathManager;
        private readonly Dictionary<string?, Metadata?> _storage = new();

        public MetadataStorage(IPathManager pathManager)
        {
            _pathManager = pathManager;
        }

        public void Add(Metadata metadata, out bool alreadyAdded)
        {
            if (!_storage.ContainsKey(metadata.ID))
            {
                _storage.Add(metadata.ID, metadata);
                alreadyAdded = false;
            }
            else
            {
                alreadyAdded = true;
            }
        }

        public void Clear()
        {
            _storage.Clear();
        }

        public Metadata Get(string id)
        {
            return _storage[id];
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
                if (item.Value.Parent.Equals(parent))
                {
                    list.Add(item.Value);
                }
            }

            return list;
        }

        public IEnumerable<string?> GetNames()
        {
            return _storage.Keys;
        }

        public void Move(Metadata md, string folder)
        {
            if (md.Type == "DocumentType")
            {
                md.Parent = folder;
                md.Version++;

                _storage[md.ID] = md; //replace metadata with changed md

                md.Save();
            }
        }

        public void Remove(Metadata tmpl)
        {
            _storage.Remove(tmpl.ID);
        }
    }
}
