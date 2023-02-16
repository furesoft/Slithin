namespace Slithin.Modules.Repository.Models;

public interface ILoadingService
{
    Task LoadNotebooksAsync();

    Task LoadTemplatesAsync();
}
