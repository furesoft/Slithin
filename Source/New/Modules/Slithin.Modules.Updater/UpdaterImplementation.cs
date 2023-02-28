using System.Runtime.InteropServices;
using NuGet.Versioning;
using Slithin.Core.MVVM;
using Slithin.Modules.Updater.Models;
using Slithin.Modules.Updater.ViewModels;

namespace Slithin.Modules.Updater;

internal class UpdaterImplementation : IUpdaterService
{
    private Dictionary<string, NuGetVersion> newModuleVersions;

    public async Task<bool> CheckForUpdate()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var uwpHelper = new DesktopBridge.Helpers();
            if (uwpHelper.IsRunningAsUwp())
            {
                return false;
            }
        }

        newModuleVersions = await UpdateRepository.GetUpdatablePackages();
        
        return newModuleVersions.Where(_=> _.Key.StartsWith("Slithin")).Any();
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
