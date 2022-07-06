using System;
using System.IO;
using System.Reflection;

namespace Slithin.Core.Services.Implementations;

public class VersionServiceImpl : IVersionService
{
    private readonly IPathManager _pathManager;

    public VersionServiceImpl(IPathManager pathManager)
    {
        _pathManager = pathManager;
    }

    public Version GetDeviceVersion()
    {
        var _ssh = ServiceLocator.Container.Resolve<ISSHService>();

        var str = _ssh.RunCommand("grep '^REMARKABLE_RELEASE_VERSION' /usr/share/remarkable/update.conf").Result;
        str = str.Replace("REMARKABLE_RELEASE_VERSION=", "").Replace("\n", "");

        return new(str);
    }

    public Version GetLocalVersion()
    {
        var path = Path.Combine(_pathManager.ConfigBaseDir, ".version");

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
