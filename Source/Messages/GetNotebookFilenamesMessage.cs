using Actress;
using Slithin.Core;

namespace Slithin.Messages
{
    public class GetNotebookFilenamesMessage : AsynchronousMessage
    {
        public GetNotebookFilenamesMessage(IReplyChannel<string[]> channel)
        {
            Channel = channel;
        }

        public Actress.IReplyChannel<string[]> Channel { get; set; }
    }
}
