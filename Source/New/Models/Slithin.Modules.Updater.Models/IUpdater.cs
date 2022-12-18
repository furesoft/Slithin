namespace Slithin.Modules.Updater.Models;

public interface IUpdaterService
{
    public Task<bool> CheckForUpdate();

    public Task StartUpdate();
}
