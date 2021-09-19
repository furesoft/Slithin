using System.Drawing.Imaging;
using System.IO;
using PdfSharpCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using Slithin.Core.Services;
using Svg;

namespace Slithin.Core.Remarkable.Rendering.Exporters
{
    public class PdfExporter : IExportProvider
    {
        private readonly IPathManager _pathManager;

        public PdfExporter(IPathManager pathManager)
        {
            _pathManager = pathManager;
        }

        public bool ExportSingleDocument => true;
        public string Title => "PDF Document";

        public bool CanHandle(Metadata md)
        {
            return md.Content.FileType == "notebook";
        }

        public bool Export(ExportOptions options, Metadata metadata, string outputPath)
        { 
            if (options.Document.IsT1)
            {
                var notebook = options.Document.AsT1;

                var document = new PdfDocument();

                document.Info.Title = metadata.VisibleName;

                for (var i = 0; i < options.PagesIndices.Count; i++)
                {
                    var pdfPage = document.AddPage();
                    using var graphics = XGraphics.FromPdfPage(pdfPage); //Dipose?

                    var page = notebook.Pages[options.PagesIndices[i]];

                    using var svgStrm = SvgRenderer.RenderPage(page, i, metadata);
                    using var pngStrm = new MemoryStream();

                    svgStrm.Seek(0, SeekOrigin.Begin);

                    var svgDoc = SvgDocument.Open<SvgDocument>(svgStrm);
                    using var bitmap = svgDoc.Draw();
                    bitmap.Save(pngStrm, ImageFormat.Png);

                    svgStrm.Close();

                    pngStrm.Seek(0, SeekOrigin.Begin);

                    graphics.DrawImage(XImage.FromStream(() => pngStrm), new XPoint(0, 0));
                }

                document.Save(outputPath);

                return true;
            }


            if (!options.Document.IsT0)
                return false;

            var filename = Path.Combine(_pathManager.NotebooksDir, metadata.ID + ".pdf");
            var doc = PdfReader.Open(File.OpenRead(filename), PdfDocumentOpenMode.Import);
            var result = new PdfDocument();

            result.Info.Title = metadata.VisibleName;

            for (var i = 0; i < options.PagesIndices.Count; i++)
            {
                var rm = metadata.Content.Pages[i];
                var rmPath = Path.Combine(_pathManager.NotebooksDir, metadata.ID, rm + ".rm");

                //todo: figure out index
                PdfPage p;
                if (!File.Exists(rmPath))
                {
                    //copy
                    p = result.AddPage(doc.Pages[i], AnnotationCopyingType.DeepCopy);
                    continue;
                }
                p = result.AddPage();

                //render
                var page = Notebook.LoadPage(File.OpenRead(rmPath));

                var psize = PageSizeConverter.ToSize(PageSize.A4);
                using var svgStrm = SvgRenderer.RenderPage(page, i, metadata, (int)psize.Width, (int)psize.Height);
                using var pngStrm = new MemoryStream();

                svgStrm.Seek(0, SeekOrigin.Begin);

                var d = SvgDocument.Open<SvgDocument>(svgStrm);
                using var bitmap = d.Draw();
                bitmap.Save(pngStrm, ImageFormat.Png);

                svgStrm.Close();

                var graphics = XGraphics.FromPdfPage(p);

                pngStrm.Seek(0, SeekOrigin.Begin);

                graphics.DrawImage(XImage.FromStream(() => pngStrm), new XPoint(0, 0));
            }

            result.Save(outputPath);

            return true;
        }
    }
}
