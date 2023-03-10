namespace Slithin.Modules.Updater.Models;

/// <summary>
/// A Service To Install Updates
/// </summary>
public interface IUpdaterService
{
    public Task<bool> CheckForUpdate();

    /// <summary>
    /// Show a download window if any module updates are available
    /// </summary>
    /// <returns></returns>
    public Task StartUpdate();
}
