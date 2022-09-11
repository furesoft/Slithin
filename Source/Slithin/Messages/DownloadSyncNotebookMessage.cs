using System.Collections.Concurrent;
using Slithin.Core.Messaging;
using Slithin.Models;

namespace Slithin.Messages;

public class DownloadSyncNotebookMessage : AsynchronousMessage
{
    public DownloadSyncNotebookMessage(ConcurrentBag<SyncNotebook> notebooks)
    {
        Notebooks = notebooks;
    }

    public ConcurrentBag<SyncNotebook> Notebooks { get; set; }
}
