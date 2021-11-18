using System.Collections.Generic;
using Slithin.Core;
using Slithin.Models;

namespace Slithin.Messages;

public class DownloadSyncNotebookMessage : AsynchronousMessage
{
    public DownloadSyncNotebookMessage(List<SyncNotebook> notebooks)
    {
        Notebooks = notebooks;
    }

    public List<SyncNotebook> Notebooks { get; set; }
}