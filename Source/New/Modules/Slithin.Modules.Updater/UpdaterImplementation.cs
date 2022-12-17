using AuroraModularis.Core;
using nUpdate.Updating;
using Slithin.Modules.Updater.Models;

namespace Slithin.Modules.Updater;

public class UpdaterImplementation : IUpdater
{
    private readonly UpdateManager _manager;
    private Container _container;

    public UpdaterImplementation(Container container)
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
