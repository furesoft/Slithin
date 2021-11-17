using Slithin.Core;
using Slithin.Core.Sync;

namespace Slithin.Messages;

public class SyncMessage : AsynchronousMessage
{
    public SyncItem Item { get; set; }
}