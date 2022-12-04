namespace Slithin.Modules.Updater.Models;

public interface IUpdater
{
    public Task<bool> CheckForUpdate();

    public void StartUpdate();
}
