namespace Slithin.Modules.Repository.Models;

public interface ILoadingService
{
    void LoadApiToken();

    void LoadNotebooks();

    void LoadTemplates();

    void LoadTools();
}
