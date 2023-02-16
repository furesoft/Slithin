namespace Slithin.Core.Sync;

public interface IRepository
{
    void AddTemplate(Template template);

    Template[] GetTemplates();

    void RemoveTemplate(Template template);
}
