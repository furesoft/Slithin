using System.Reflection;
using AuroraModularis.Core;
using Slithin.Modules.Device.Models;
using Slithin.Modules.Repository.Models;

namespace Slithin.Modules.Repository;

internal class VersionServiceImpl : IVersionService
{
    private readonly Container _container;

    public VersionServiceImpl(Container container)
    {
        _container = container;
    }

    public Version GetDeviceVersion()
    {
        var str = _container.Resolve<IRemarkableDevice>().RunCommand("grep '^REMARKABLE_RELEASE_VERSION' /usr/share/remarkable/update.conf").Result;
        str = str.Replace("REMARKABLE_RELEASE_VERSION=", "").Replace("\n", "");

        return new(str);
    }

    public void UpdateVersion(Version version)
    {
        var pathManager = _container.Resolve<IPathManager>();

        if (!Directory.Exists(pathManager.ConfigBaseDir))
        {
            pathManager.Init();
            pathManager.InitDeviceDirectory();
        }

        File.WriteAllText(Path.Combine(pathManager.ConfigBaseDir, ".version"), version.ToString());
    }

    public Version GetLocalVersion()
    {
        var versionPath = Path.Combine(_container.Resolve<IPathManager>().ConfigBaseDir, ".version");

        return File.Exists(versionPath) ? new Version(File.ReadAllText(versionPath)) : new(0, 0, 0, 0);
    }

    public Version GetSlithinVersion()
    {
        return Assembly.GetExecutingAssembly().GetName().Version;
    }

    public bool HasLocalVersion()
    {
        var versionFile = Path.Combine(_container.Resolve<IPathManager>().ConfigBaseDir, ".version");

        return File.Exists(versionFile);
    }
}
