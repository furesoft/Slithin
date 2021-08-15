using System.Drawing.Imaging;
using System.IO;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using Svg;

namespace Slithin.Core.Remarkable.Rendering.Exporters
{
    public class PdfExporter : IExportProvider
    {
        public bool ExportSingleDocument => true;
        public string Title => "PDF Document";

        public void Export(Notebook notebook, Metadata metadata, string outputPath)
        {
            var document = new PdfDocument();

            for (var i = 0; i < notebook.Pages.Count; i++)
            {
                var pdfPage = document.AddPage();
                var graphics = XGraphics.FromPdfPage(pdfPage);

                var page = notebook.Pages[i];

                var svgStrm = SvgRenderer.RenderPage(page, i, metadata);
                var pngStrm = new MemoryStream();

                var doc = SvgDocument.Open<SvgDocument>(svgStrm);
                var bitmap = doc.Draw();
                bitmap.Save(pngStrm, ImageFormat.Png);

                svgStrm.Close();
                graphics.DrawImage(XImage.FromStream(() => pngStrm), new XPoint(0, 0));
            }

            document.Save(outputPath);
        }
    }
}
