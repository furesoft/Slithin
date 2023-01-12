using Slithin.Entities.Remarkable;

namespace Slithin.Modules.Repository.Models;

public interface ITemplateStorage
{
    Template[] Templates { get; set; }

    void AppendTemplate(Template template);

    void Remove(Template tmpl);

    void Save();

    void Load();

    Task LoadTemplateAsync(Template template);
}
