using System;
using System.IO;

namespace Slithin.Core.Services.Implementations
{
    public class VersionServiceImpl : IVersionService
    {
        private readonly IPathManager _pathManager;

        public VersionServiceImpl(IPathManager pathManager)
        {
            _pathManager = pathManager;
        }

        public Version GetDeviceVersion()
        {
            var str = ServiceLocator.Client.RunCommand("grep '^REMARKABLE_RELEASE_VERSION' /usr/share/remarkable/update.conf").Result;
            str = str.Replace("REMARKABLE_RELEASE_VERSION=", "").Replace("\n", "");

            return new(str);
        }

        public Version GetLocalVersion()
        {
            return new Version(File.ReadAllText(Path.Combine(_pathManager.ConfigBaseDir, ".version")));
        }
    }
}
