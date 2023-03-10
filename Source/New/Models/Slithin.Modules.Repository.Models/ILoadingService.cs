namespace Slithin.Modules.Repository.Models;

/// <summary>
/// A service to load data from filesystem into memory
/// </summary>
public interface ILoadingService
{
    Task LoadNotebooksAsync();

    Task LoadTemplatesAsync();
}
