using AuroraModularis.Core;
using nUpdate.Updating;
using Slithin.Modules.Updater.Models;

namespace Slithin.Modules.Updater;

internal class UpdaterImplementation : IUpdaterService
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
        if (_manager == null)
        {
            return Task.FromResult(false);
        }

        return _manager.SearchForUpdatesAsync();
    }

    public async Task StartUpdate()
    {
        await _manager.DownloadPackagesAsync();
        if (_manager.ValidatePackages())
            _manager.InstallPackage();
    }
}
