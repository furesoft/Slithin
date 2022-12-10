using Slithin.Core.Remarkable;
using Slithin.Core.Remarkable.Models;

namespace Slithin.Core.Sync;

public interface IRepository
{
    void AddTemplate(Template template);

    Template[] GetTemplates();

    void RemoveTemplate(Template template);
}
