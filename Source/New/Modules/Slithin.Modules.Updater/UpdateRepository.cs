using System.Diagnostics;
using System.Reflection;
using Avalonia.Threading;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace Slithin.Modules.Updater;

internal class UpdateRepository
{
    public static Dictionary<string, string> GetModuleVersions()
    {
        var versions = new Dictionary<string, string>();

        foreach (var file in Directory.GetFiles(Environment.CurrentDirectory, "*.dll"))
        {
            if (!Path.GetFileName(file).Contains("Slithin"))
            {
                continue;
            }

            var assembly = Assembly.LoadFile(file);
            var assemblyName = assembly.GetName();

            versions.Add(assembly.GetName().Name, assemblyName.Version.ToString());
        }

        return versions;
    }

    public static async Task<IEnumerable<PackageDependency>> GetNugetPackages()
    {
        var version = await GetLatestVersion();

        var cache = new SourceCacheContext();
        var source = new PackageSource("https://api.nuget.org/v3/index.json");
        var providers = Repository.Provider.GetCoreV3();
        var repository = new SourceRepository(source, providers);
        var resource = await repository.GetResourceAsync<FindPackageByIdResource>();
        var packages = await resource.GetDependencyInfoAsync("Slithin", version, cache, NullLogger.Instance, CancellationToken.None);

        return packages.DependencyGroups[0].Packages;
    }

    private static async Task<NuGetVersion> GetLatestVersion()
    {
        Debug.WriteLine(Dispatcher.UIThread.CheckAccess());

        var cache = new SourceCacheContext();
        var source = new PackageSource("https://www.myget.org/F/slithin/api/v3/index.json");
        var providers = Repository.Provider.GetCoreV3();
        var repository = new SourceRepository(source, providers);

        var resource = repository.GetResource<FindPackageByIdResource>();
        var versions = await resource.GetAllVersionsAsync(
            "Slithin",
            cache,
            NullLogger.Instance,
            CancellationToken.None);

        return versions.Last();
    }
}
