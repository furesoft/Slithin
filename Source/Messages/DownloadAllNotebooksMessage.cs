using Actress;
using Slithin.Core;

namespace Slithin.Messages
{
    public class DownloadAllNotebooksMessage : AsynchronousMessage
    {
        public DownloadAllNotebooksMessage(IReplyChannel<string[]> channel)
        {
            Channel = channel;
        }

        public Actress.IReplyChannel<string[]> Channel { get; set; }
    }
}
