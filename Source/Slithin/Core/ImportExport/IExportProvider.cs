using System;
using Slithin.Core.Remarkable.Exporting.Rendering;
using Slithin.Core.Remarkable.Models;

namespace Slithin.Core.ImportExport;

public interface IExportProvider
{
    bool ExportSingleDocument { get; }
    string Title { get; }

    bool CanHandle(Metadata md);

    bool Export(ExportOptions options, Metadata metadata, string outputPath, IProgress<int> progress);
}
