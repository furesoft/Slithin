using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Newtonsoft.Json;
using Slithin.Core.Services;

namespace Slithin.Core.Remarkable
{
    public class Metadata : IEqualityComparer<Metadata>
    {
        [JsonIgnore]
        public ContentFile Content { get; set; }

        [JsonProperty("deleted")]
        public bool Deleted { get; set; }

        [JsonIgnore]
        public string ID { get; internal set; }

        [JsonProperty("lastOpenedPage")]
        public int LastOpenedPage { get; set; }

        [JsonIgnore]
        public PageData PageData { get; set; } = new();

        [JsonProperty("parent")]
        public string? Parent { get; set; }

        [JsonProperty("pinned")]
        public bool Pinned { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("visibleName")]
        public string? VisibleName { get; set; }

        public bool Equals(Metadata x, Metadata y)
        {
            return x.Content.Equals(x.Content) &&
                x.Deleted == y.Deleted &&
                x.ID.Equals(y.ID) &&
                x.LastOpenedPage == y.LastOpenedPage &&
                x.Parent.Equals(y.Parent) &&
                x.Type.Equals(y.Type) &&
                x.Version == y.Version &&
                x.VisibleName.Equals(y.VisibleName);
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

            File.WriteAllText(Path.Combine(pathManager.NotebooksDir, ID + ".metadata"), JsonConvert.SerializeObject(md, Formatting.Indented));

            if (new ContentFile?(Content).HasValue)
            {
                File.WriteAllText(Path.Combine(pathManager.NotebooksDir, ID + ".content"), JsonConvert.SerializeObject(md.Content, Formatting.Indented));
            }
        }

        public override string ToString()
        {
            return VisibleName;
        }
    }
}
