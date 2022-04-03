using System.Collections.Generic;
using Slithin.Core;
using Slithin.Models;
using Slithin.Core.Messaging;

namespace Slithin.Messages;

public class DownloadSyncNotebookMessage : AsynchronousMessage
{
    public DownloadSyncNotebookMessage(List<SyncNotebook> notebooks)
    {
        Notebooks = notebooks;
    }

    public List<SyncNotebook> Notebooks { get; set; }
}