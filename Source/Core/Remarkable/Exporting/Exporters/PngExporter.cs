using System.Drawing.Imaging;
using System.IO;
using Svg;

namespace Slithin.Core.Remarkable.Rendering.Exporters
{
    public class PngExporter : IExportProvider
    {
        public bool ExportSingleDocument => false;
        public string Title => "PNG Graphics";

        public void Export(Notebook notebook, Metadata metadata, string outputPath)
        {
            for (var i = 0; i < notebook.Pages.Count; i++)
            {
                var page = notebook.Pages[i];

                var svgStrm = SvgRenderer.RenderPage(page, i, metadata);

                var doc = SvgDocument.Open<SvgDocument>(svgStrm);
                var bitmap = doc.Draw();
                bitmap.Save(Path.Combine(outputPath, i + ".png"), ImageFormat.Png);

                svgStrm.Close();
            }
        }
    }
}
