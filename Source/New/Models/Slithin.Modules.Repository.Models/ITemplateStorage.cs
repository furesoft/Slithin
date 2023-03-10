using Slithin.Entities.Remarkable;

namespace Slithin.Modules.Repository.Models;

/// <summary>
/// A service to work with the templates.json config file
/// </summary>
public interface ITemplateStorage
{
    Template[] Templates { get; set; }

    void AppendTemplate(Template template);

    void Remove(Template tmpl);

    void Save();

    void Load();

    Task LoadTemplateAsync(Template template);
}
