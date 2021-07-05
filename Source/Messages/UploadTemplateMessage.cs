using Slithin.Core;
using Slithin.Core.Remarkable;

namespace Slithin.Messages
{
    public class UploadTemplateMessage : AsynchronousMessage
    {
        public byte[] Raw { get; set; }
        public Template Template { get; set; }
    }
}
