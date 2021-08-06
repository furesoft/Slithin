using Slithin.Core;

namespace Slithin.Messages
{
    public class ShowStatusMessage : AsynchronousMessage
    {
        public string Message { get; set; }
    }
}
