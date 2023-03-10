using Slithin.Entities.Remarkable;

namespace Slithin.Modules.Repository.Models;

/// <summary>
/// A service to work with notebook metadata
/// </summary>
public interface IMetadataRepository
{
    void AddMetadata(Metadata metadata, out bool alreadyAdded);

    void SaveToDisk(Metadata metadata);

    void Clear();

    IEnumerable<Metadata> GetByParent(string parent);

    Metadata GetMetadata(string id);

    void Move(Metadata md, string folder);

    Metadata Load(string id);

    void Upload(Metadata md, bool onlyMetadata = false);

    void Remove(Metadata md);
}
