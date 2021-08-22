﻿using System;
using System.IO;
using Renci.SshNet;

namespace Slithin.Core.Services.Implementations
{
    public class VersionServiceImpl : IVersionService
    {
        private readonly IPathManager _pathManager;
        private readonly SshClient _sshClient;

        public VersionServiceImpl(IPathManager pathManager, SshClient sshClient)
        {
            _pathManager = pathManager;
            _sshClient = sshClient;
        }

        public Version GetDeviceVersion()
        {
            var str = _sshClient.RunCommand("grep '^REMARKABLE_RELEASE_VERSION' /usr/share/remarkable/update.conf").Result;
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
    }
}
