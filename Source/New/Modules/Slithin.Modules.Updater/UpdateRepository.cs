using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using MoreLinq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace Slithin.Modules.Updater;

internal class UpdateRepository
{
    private static readonly SourceRepository repository;
    private static SourceCacheContext cache;
    private static PackageSource source;
    private static CancellationTokenSource cts;

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
            if (!Path.GetFileNameWithoutExtension(file).Contains("Slithin"))
            {
                continue;
            }

            var assembly = Assembly.LoadFile(file);
            var assemblyName = assembly.GetName();

            versions.Add(assembly.GetName().Name, assemblyName.Version.ToString());
        }

        return versions;
    }

    public static async Task<IEnumerable<PackageDependency>> GetNugetBaseDependencyPackages()
    {
        var version = await GetLatestVersion();

        var resource = await repository.GetResourceAsync<FindPackageByIdResource>();
        var packages = await resource.GetDependencyInfoAsync("Slithin", version, cache, NullLogger.Instance, cts.Token);

        return packages.DependencyGroups[0].Packages.SelectMany(dep => GetNugetSubDependencyPackages(resource, dep).Result)
            .Concat(new[] { new PackageDependency("Slithin", new(version)) });
    }

    private static async Task<IEnumerable<PackageDependency>> GetNugetSubDependencyPackages(FindPackageByIdResource resource, PackageDependency dependency)
    {
        var packages = await resource.GetDependencyInfoAsync(dependency.Id, dependency.VersionRange.MinVersion, cache, NullLogger.Instance, cts.Token);

        return packages.DependencyGroups[0].Packages.SelectMany(dep => GetNugetSubDependencyPackages(resource, dep).Result).Concat(dependency);
    }

    public static async Task<Dictionary<string, NuGetVersion>> GetUpdatablePackages()
    {
        var localVersions = GetModuleVersions();
        var remoteVersions = await GetNugetBaseDependencyPackages();
        var result = new Dictionary<string, NuGetVersion>();

        foreach (var remoteVersion in remoteVersions)
        {
            var remoteMinVersion = remoteVersion.VersionRange.MinVersion;
            if (!localVersions.ContainsKey(remoteVersion.Id))
            {
                result.Add(remoteVersion.Id, remoteMinVersion);
                continue;
            }

            var localVersion = localVersions[remoteVersion.Id];

            if (new Version(localVersion) < remoteMinVersion.Version)
            {
                result.Add(remoteVersion.Id, remoteMinVersion);
            }
        }

        return result;
    }

    public static async Task DownloadPackage(string id, NuGetVersion version, IProgress<bool> progress)
    {
        var pkgStream = await GetNupkgStream(id, version);

        var reader = new PackageArchiveReader(pkgStream);
        var libItemsGroups = await reader.GetLibItemsAsync(cts.Token);

        var basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "SlithinUpdate");
        foreach (var item in libItemsGroups.First().Items)
        {
            var normalizedItem = Path.GetFileName(item);
            var filePath = Path.Combine(basePath, normalizedItem);
            reader.ExtractFile(item, filePath, NullLogger.Instance);
        }

        progress.Report(true);
    }

    private static async Task<MemoryStream> GetNupkgStream(string id, NuGetVersion version)
    {
        var resource = repository.GetResource<FindPackageByIdResource>();
        var pkgStream = new MemoryStream();

        await resource.CopyNupkgToStreamAsync(id, version, pkgStream, cache, NullLogger.Instance, cts.Token);
        return pkgStream;
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
