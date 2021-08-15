using Slithin.Core.Remarkable;
using Slithin.Core.Remarkable.Rendering;

namespace Slithin.Core
{
    public interface IExportProvider
    {
        string Extension { get; }

        void Export(Notebook notebook, Metadata metadata, string outputPath);
    }
}
