using Slithin.Entities.Remarkable;

namespace Slithin.Core.ImportExport;

public interface IExportProvider
{
    bool ExportSingleDocument { get; }
    string Title { get; }

    bool CanHandle(Metadata md);

    bool Export(ExportOptions options, Metadata metadata, string outputPath, IProgress<int> progress);
}
