using System.Reflection;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace Slithin.Modules.Updater;

internal class UpdateRepository
{
    private static SourceCacheContext cache;
    private static PackageSource source;
    private static readonly SourceRepository repository;
    private static readonly CancellationTokenSource cts; 

    static UpdateRepository()
    {
        cts = new();
        cache = new();
        source = new("https://api.nuget.org/v3/index.json");
        var providers = Repository.Provider.GetCoreV3();
        
        repository = new(source, providers);
    }
    
    public static Dictionary<string, string> GetModuleVersions()
    {
        var versions = new Dictionary<string, string>();

        foreach (var file in Directory.GetFiles(Environment.CurrentDirectory, "*.dll"))
        {
            var assembly = Assembly.LoadFile(file);
            var assemblyName = assembly.GetName();

            versions.Add(assembly.GetName().Name, assemblyName.Version.ToString());
        }

        return versions;
    }

    public static async Task<IEnumerable<PackageDependency>> GetNugetPackages()
    {
        var version = await GetLatestVersion();

        var resource = await repository.GetResourceAsync<FindPackageByIdResource>();
        var packages = await resource.GetDependencyInfoAsync("Slithin", version, cache, NullLogger.Instance, cts.Token);

        return packages.DependencyGroups[0].Packages;
    }

    private static async Task<NuGetVersion> GetLatestVersion()
    {
        var resource = await repository.GetResourceAsync<FindPackageByIdResource>(cts.Token);
        var versions = await resource.GetAllVersionsAsync(
            "Slithin",
            cache,
            NullLogger.Instance,
            cts.Token);

        return versions.Last();
    }
}
