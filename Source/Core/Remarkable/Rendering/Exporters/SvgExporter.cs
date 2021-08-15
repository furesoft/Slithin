using System.IO;
using Svg;

namespace Slithin.Core.Remarkable.Rendering.Exporters
{
    public class PngExporter : IExportProvider
    {
        public string Extension => ".png";

        public void Export(Notebook notebook, Metadata metadata, string outputPath)
        {
            for (var i = 0; i < notebook.Pages.Count; i++)
            {
                var page = notebook.Pages[i];

                var svgStrm = SvgRenderer.RenderPage(page, i, metadata);

                var doc = SvgDocument.Open<SvgDocument>(svgStrm);
                var bitmap = doc.Draw();
                bitmap.Save(Path.Combine(outputPath, i + ".svg"));

                svgStrm.Close();
            }
        }
    }

    public class SvgExporter : IExportProvider
    {
        public string Extension => ".svg";

        public void Export(Notebook notebook, Metadata metadata, string outputPath)
        {
            for (var i = 0; i < notebook.Pages.Count; i++)
            {
                var page = notebook.Pages[i];

                var svgStrm = SvgRenderer.RenderPage(page, i, metadata);
                var outputStrm = File.OpenWrite(Path.Combine(outputPath, i + ".svg"));

                svgStrm.CopyTo(outputStrm);

                svgStrm.Close();
                outputStrm.Close();
            }
        }
    }
}
