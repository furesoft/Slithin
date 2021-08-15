using System.IO;

namespace Slithin.Core
{
    public interface IExportProvider
    {
        string Extension { get; }

        Stream Export(Stream svgStream);
    }
}
