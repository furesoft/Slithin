namespace Slithin.Core.Services;

public interface ILoadingService
{
    void LoadApiToken();

    void LoadNotebooks();

    void LoadScreens();

    void LoadTemplates();

    void LoadTools();
}
