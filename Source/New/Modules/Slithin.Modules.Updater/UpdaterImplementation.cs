using System.Diagnostics;
using System.Runtime.InteropServices;
using NuGet.Versioning;
using Slithin.Modules.Updater.Models;

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

    public async Task StartUpdate()
    {
        foreach (var package in newModuleVersions)
        {
            await UpdateRepository.DownloadPackage(package.Key, package.Value);
        }
        
        Process.Start(new ProcessStartInfo("dotnet", typeof(UpdateInstaller.Program).Assembly.Location + " " + Path.Combine(Environment.CurrentDirectory, "Slithin.dll")));

    }
}
