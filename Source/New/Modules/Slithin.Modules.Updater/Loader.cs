using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Resolver;
using NuGet.Versioning;

namespace Slithin.Modules.Updater;

public class Loader
{
    public class ExtensionConfiguration
    {
        public string Package { get; set; }
        public string Version { get; set; }
        public bool PreRelease { get; set; }
    }

    public async Task<IEnumerable<SourcePackageDependencyInfo>> LoadExtensions(NuGetVersion nuGetVersion)
    {
        // Define a source provider, with nuget, plus my own feed.
        var sourceProvider = new PackageSourceProvider(NullSettings.Instance, new[]
        {
            new PackageSource("https://api.nuget.org/v3/index.json"),
        });

        // Establish the source repository provider; the available providers come from our custom settings.
        var sourceRepositoryProvider = new SourceRepositoryProvider(sourceProvider, Repository.Provider.GetCoreV3());

        // Get the list of repositories.
        var repositories = sourceRepositoryProvider.GetRepositories();

        // Disposable source cache.
        using var sourceCacheContext = new SourceCacheContext();

        // You should use an actual logger here, this is a NuGet ILogger instance.
        var logger = new NullLogger();

        // My extension configuration:
        var extensions = new[]
        {
            new ExtensionConfiguration
            {
                Package = "Slithin",
                PreRelease = false,
                Version = nuGetVersion.ToString()
            }
        };

        // Replace this with a proper cancellation token.
        var cancellationToken = CancellationToken.None;

        // The framework we're using.
        var targetFramework = NuGetFramework.ParseFolder("net7.0");
        var allPackages = new HashSet<SourcePackageDependencyInfo>();

        foreach (var ext in extensions)
        {
            var packageIdentity = await GetPackageIdentity(ext, sourceCacheContext, logger, repositories, cancellationToken);

            if (packageIdentity is null)
            {
                throw new InvalidOperationException($"Cannot find package {ext.Package}.");
            }
                
            await GetPackageDependencies(packageIdentity, sourceCacheContext, targetFramework, logger, repositories, allPackages, cancellationToken);
        }

        return GetPackagesToInstall(sourceRepositoryProvider, logger, extensions, allPackages);
    }

    private IEnumerable<SourcePackageDependencyInfo> GetPackagesToInstall(SourceRepositoryProvider sourceRepositoryProvider, 
        ILogger logger, IEnumerable<ExtensionConfiguration> extensions, 
        HashSet<SourcePackageDependencyInfo> allPackages)
    {
        // Create a package resolver context (this is used to help figure out which actual package versions to install).
        var resolverContext = new PackageResolverContext(
            DependencyBehavior.Lowest,
            extensions.Select(x => x.Package),
            Enumerable.Empty<string>(),
            Enumerable.Empty<PackageReference>(),
            Enumerable.Empty<PackageIdentity>(),
            allPackages,
            sourceRepositoryProvider.GetRepositories().Select(s => s.PackageSource),
            logger);

        var resolver = new PackageResolver();

        // Work out the actual set of packages to install.
        var packagesToInstall = resolver.Resolve(resolverContext, CancellationToken.None)
            .Select(p => allPackages.Single(x => PackageIdentityComparer.Default.Equals(x, p)));
        
        return packagesToInstall.Where(_=> !RuntimeProvidedPackages.IsPackageProvidedByRuntime(_.Id));
    }

    private async Task<PackageIdentity> GetPackageIdentity(
        ExtensionConfiguration extConfig, SourceCacheContext cache, ILogger nugetLogger,
        IEnumerable<SourceRepository> repositories, CancellationToken cancelToken)
    {
        foreach (var sourceRepository in repositories)
        {
            // Get a 'resource' from the repository.
            var findPackageResource = await sourceRepository.GetResourceAsync<FindPackageByIdResource>();

            // Get the list of all available versions of the package in the repository.
            var allVersions = await findPackageResource.GetAllVersionsAsync(extConfig.Package, cache, nugetLogger, cancelToken);

            NuGetVersion selected;

            // Have we specified a version range?
            if (extConfig.Version != null)
            {
                if (!VersionRange.TryParse(extConfig.Version, out var range))
                {
                    throw new InvalidOperationException("Invalid version range provided.");
                }

                // Find the best package version match for the range.
                // Consider pre-release versions, but only if the extension is configured to use them.
                var bestVersion = range.FindBestMatch(allVersions.Where(v => extConfig.PreRelease || !v.IsPrerelease));

                selected = bestVersion;
            }
            else
            {
                // No version; choose the latest, allow pre-release if configured.
                selected = allVersions.LastOrDefault(v => v.IsPrerelease == extConfig.PreRelease);
            }

            if (selected is object)
            {
                return new PackageIdentity(extConfig.Package, selected);
            }
        }

        return null;
    }

    /// <summary>
    /// Searches the package dependency graph for the chain of all packages to install.
    /// </summary>
    private async Task GetPackageDependencies(PackageIdentity package, SourceCacheContext cacheContext, NuGetFramework framework, 
        ILogger logger, IEnumerable<SourceRepository> repositories,
        ISet<SourcePackageDependencyInfo> availablePackages, CancellationToken cancelToken)
    {
        // Don't recurse over a package we've already seen.
        if (availablePackages.Contains(package))
        {
            return;
        }

        foreach (var sourceRepository in repositories)
        {
            // Get the dependency info for the package.
            var dependencyInfoResource = await sourceRepository.GetResourceAsync<DependencyInfoResource>();
            var dependencyInfo = await dependencyInfoResource.ResolvePackage(
                package,
                framework,
                cacheContext,
                logger,
                cancelToken);

            // No info for the package in this repository.
            if (dependencyInfo == null)
            {
                continue;
            }


            // Filter the dependency info.
            // Don't bring in any dependencies that are provided by the host.
            var actualSourceDep = new SourcePackageDependencyInfo(
                dependencyInfo.Id,
                dependencyInfo.Version,
                dependencyInfo.Dependencies,//.Where(dep => !DependencySuppliedByHost(hostDependencies, dep)),
                dependencyInfo.Listed,
                dependencyInfo.Source);

            availablePackages.Add(actualSourceDep);

            // Recurse through each package.
            foreach (var dependency in actualSourceDep.Dependencies)
            {
                await GetPackageDependencies(
                    new PackageIdentity(dependency.Id, dependency.VersionRange.MinVersion),
                    cacheContext,
                    framework,
                    logger,
                    repositories,
                    availablePackages,
                    cancelToken);
            }

            break;
        }
    }

    /*private bool DependencySuppliedByHost(DependencyContext hostDependencies, PackageDependency dep)
    {
        //if(RuntimeProvidedPackages.IsPackageProvidedByRuntime(dep.Id))
        {
          //  return true;
        }

        // See if a runtime library with the same ID as the package is available in the host's runtime libraries.
        var runtimeLib = hostDependencies.RuntimeLibraries.FirstOrDefault(r => r.Name == dep.Id);

        if (runtimeLib is object)
        {
            // What version of the library is the host using?
            var parsedLibVersion = NuGetVersion.Parse(runtimeLib.Version);

            if (parsedLibVersion.IsPrerelease)
            {
                // Always use pre-release versions from the host, otherwise it becomes
                // a nightmare to develop across multiple active versions.
                return true;
            }
            else
            {
                // Does the host version satisfy the version range of the requested package?
                // If so, we can provide it; otherwise, we cannot.
                return dep.VersionRange.Satisfies(parsedLibVersion);
            }
        }

        return false;
    }
    */
}
