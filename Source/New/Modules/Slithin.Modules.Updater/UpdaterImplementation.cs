using NuGet.Versioning;
using Slithin.Core.MVVM;
using Slithin.Modules.Updater.Models;
using Slithin.Modules.Updater.ViewModels;

namespace Slithin.Modules.Updater;

internal class UpdaterImplementation : IUpdaterService
{
    private Dictionary<string, NuGetVersion> newModuleVersions;

    public UpdaterImplementation()
    {
    }

    public async Task<bool> CheckForUpdate()
    {
        newModuleVersions = await UpdateRepository.GetUpdatablePackages();
        return newModuleVersions.Count > 0;
    }

    public Task StartUpdate()
    {
        var window = new UpdaterWindow();
        var vm = new UpdaterViewModel(newModuleVersions);

        BaseViewModel.ApplyViewModel(window, vm);

        window.Show();

        return Task.CompletedTask;
    }
}
