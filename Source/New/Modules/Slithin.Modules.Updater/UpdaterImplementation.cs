using NuGet.Versioning;
using Slithin.Modules.Updater.Models;

namespace Slithin.Modules.Updater;

internal class UpdaterImplementation : IUpdaterService
{
    private readonly Dictionary<string, NuGetVersion> newModuleVersions;

    public UpdaterImplementation()
    {
        newModuleVersions = UpdateRepository.GetUpdatablePackages().Result;
    }
    public Task<bool> CheckForUpdate()
    {
        return Task.FromResult(newModuleVersions.Count > 0);
    }

    public Task StartUpdate()
    {
        new UpdaterWindow().Show();

        return Task.CompletedTask;
    }
}
