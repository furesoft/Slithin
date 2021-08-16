using Slithin.Core.Remarkable;
using Slithin.Core.Remarkable.Rendering;

namespace Slithin.Core
{
    public interface IExportProvider
    {
        bool ExportSingleDocument { get; }
        string Title { get; }

        void Export(ExportOptions options, Metadata metadata, string outputPath);
    }
}
