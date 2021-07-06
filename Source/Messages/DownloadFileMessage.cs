using Slithin.Core;

namespace Slithin.Messages
{
    public class DownloadFileMessage : AsynchronousMessage
    {
        public string LocalPath { get; set; }
        public string RemotePath { get; set; }
    }
}
