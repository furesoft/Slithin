using Slithin.Entities;

namespace Slithin.Modules.Repository.Models;

public interface IRepository
{
    void AddTemplate(Template template);

    Template[] GetTemplates();

    void RemoveTemplate(Template template);
}
