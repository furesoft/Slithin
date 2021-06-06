using Renci.SshNet;

namespace Slithin.Core
{
    public static class ServiceLocator
    {
        public static SshClient Client;
        public static ScpClient Scp;
    }
}