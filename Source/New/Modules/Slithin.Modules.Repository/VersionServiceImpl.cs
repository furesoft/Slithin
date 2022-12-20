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

    public Version GetLocalVersion()
    {
        var path = Path.Combine(_container.Resolve<IPathManager>().ConfigBaseDir, ".version");

        if (File.Exists(path))
        {
            return new Version(File.ReadAllText(path));
        }

        return new Version(0, 0, 0, 0);
    }

    public Version GetSlithinVersion()
    {
        return Assembly.GetExecutingAssembly().GetName().Version;
    }
}
