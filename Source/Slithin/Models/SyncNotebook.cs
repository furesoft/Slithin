using System;
using System.Collections.Generic;
using Slithin.Core.Remarkable;

namespace Slithin.Models;

public class SyncNotebook
{
    public IEnumerable<string> Directories { get; set; } = Array.Empty<string>();
    public IEnumerable<string> Files { get; set; } = Array.Empty<string>();
    public Metadata Metadata { get; set; }
}