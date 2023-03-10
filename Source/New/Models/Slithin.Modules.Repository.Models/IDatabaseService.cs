namespace Slithin.Modules.Repository.Models;

/// <summary>
/// A service to work with the slithin database
/// </summary>
public interface IDatabaseService : IDisposable
{
    DatabaseAccessor GetDatabase();
}
