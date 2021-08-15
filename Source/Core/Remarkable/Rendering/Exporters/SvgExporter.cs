using System.IO;

namespace Slithin.Core.Remarkable.Rendering.Exporters
{
    public class SvgExporter : IExportProvider
    {
        public string Extension => ".svg";

        public Stream Export(Stream svgStream)
        {
            return svgStream;
        }
    }
}
