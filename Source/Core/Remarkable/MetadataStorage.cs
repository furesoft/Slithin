using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Slithin.Core.Remarkable
{
    public class MetadataStorage
    {
        public static MetadataStorage Local = new();

        private readonly Dictionary<string?, Metadata?> _storage = new();

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
            foreach (var item in _storage)
            {
                if (item.Value.Parent.Equals(parent))
                {
                    yield return item.Value;
                }
            }
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
                _storage[md.ID] = md; //replace metadata with changed md

                File.WriteAllText(Path.Combine(ServiceLocator.NotebooksDir, md.ID + ".metadata"), JsonConvert.SerializeObject(md, Formatting.Indented));
            }
        }

        public void Remove(Metadata tmpl)
        {
            _storage.Remove(tmpl.ID);
        }
    }
}
