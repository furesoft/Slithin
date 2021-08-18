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
                    var graphics = XGraphics.FromPdfPage(pdfPage);

                    var page = notebook.Pages[options.PagesIndices[i]];

                    var svgStrm = SvgRenderer.RenderPage(page, i, metadata);
                    var pngStrm = new MemoryStream();

                    svgStrm.Seek(0, SeekOrigin.Begin);

                    var doc = SvgDocument.Open<SvgDocument>(svgStrm);
                    var bitmap = doc.Draw();
                    bitmap.Save(pngStrm, ImageFormat.Png);

                    svgStrm.Close();

                    pngStrm.Seek(0, SeekOrigin.Begin);

                    graphics.DrawImage(XImage.FromStream(() => pngStrm), new XPoint(0, 0));
                }

                document.Save(outputPath);

                return true;
            }
            else if (options.Document.IsT0)
            {
                var filename = Path.Combine(_pathManager.NotebooksDir, metadata.ID + ".pdf");
                var doc = PdfReader.Open(File.OpenRead(filename), PdfDocumentOpenMode.Import);
                var result = new PdfDocument();

                result.Info.Title = metadata.VisibleName;

                for (var i = 0; i < options.PagesIndices.Count; i++)
                {
                    var rm = metadata.Content.Pages[i];
                    var rmPath = Path.Combine(_pathManager.NotebooksDir, metadata.ID, rm + ".rm");

                    //todo: figure out index

                    if (File.Exists(rmPath))
                    {
                        var p = result.AddPage();

                        //render
                        var page = Notebook.LoadPage(File.OpenRead(rmPath));

                        var psize = PageSizeConverter.ToSize(PageSize.A4);
                        var svgStrm = SvgRenderer.RenderPage(page, i, metadata, (int)psize.Width, (int)psize.Height);
                        var pngStrm = new MemoryStream();

                        svgStrm.Seek(0, SeekOrigin.Begin);

                        var d = SvgDocument.Open<SvgDocument>(svgStrm);
                        var bitmap = d.Draw();
                        bitmap.Save(pngStrm, ImageFormat.Png);

                        svgStrm.Close();

                        var graphics = XGraphics.FromPdfPage(p);

                        pngStrm.Seek(0, SeekOrigin.Begin);

                        graphics.DrawImage(XImage.FromStream(() => pngStrm), new XPoint(0, 0));
                    }
                    else
                    {
                        //copy
                        var p = result.AddPage(doc.Pages[i], AnnotationCopyingType.DeepCopy);
                    }
                }

                result.Save(outputPath);

                return true;
            }

            return false;
        }
    }
}
