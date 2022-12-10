using AuroraModularis;
using nUpdate.Updating;
using Slithin.Modules.Updater.Models;

namespace Slithin.Core.Updates;

public class UpdaterImplementation : IUpdater
{
    private readonly UpdateManager _manager;
    private TinyIoCContainer _container;

    public UpdaterImplementation(TinyIoCContainer container)
    {
        _container = container;
        //_manager = new(new Uri(), pubK);
    }

    public Task<bool> CheckForUpdate()
    {
        return _manager.SearchForUpdatesAsync();
    }

    public async Task StartUpdate()
    {
        await _manager.DownloadPackagesAsync();
        if (_manager.ValidatePackages())
            _manager.InstallPackage();
    }
}
