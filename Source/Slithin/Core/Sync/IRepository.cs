using Slithin.Core.Remarkable;

namespace Slithin.Core.Sync;

public interface IRepository
{
    void Add(Template template);

    Template[] GetTemplates();

    void Remove(Template template);
}