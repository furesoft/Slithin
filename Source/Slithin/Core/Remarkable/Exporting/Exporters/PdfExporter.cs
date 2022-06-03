using System;
using System.Drawing.Imaging;
using System.IO;
using PdfSharpCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using Slithin.Core.FeatureToggle;
using Slithin.Core.ImportExport;
using Slithin.Core.Remarkable.Exporting.Rendering;
using Slithin.Core.Remarkable.Models;
using Slithin.Core.Services;
using Svg;
using SvgRenderer = Slithin.Core.Remarkable.Exporting.Rendering.SvgRenderer;

namespace Slithin.Core.Remarkable.Exporting.Exporters;

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
        return Feature<Features.ExportPdfFeature>.IsEnabled && (md.Content.FileType == "notebook" || md.Content.FileType == "pdf");
    }

    public bool Export(ExportOptions options, Metadata metadata, string outputPath, IProgress<int> progress)
    {
        if (options.Document.IsT1)
        {
            var notebook = options.Document.AsT1;

            var document = new PdfDocument();

            document.Info.Title = metadata.VisibleName;

            for (var i = 0; i < options.PagesIndices.Count; i++)
            {
                var percent = (int)((float)i / (float)options.PagesIndices.Count * 100);

                var pdfPage = document.AddPage();
                pdfPage.Size = PageSize.Letter;

                var graphics = XGraphics.FromPdfPage(pdfPage);

                var page = notebook.Pages[options.PagesIndices[i]];

                var svgStrm = SvgRenderer.RenderPage(page, i, metadata);
                var pngStrm = new MemoryStream();

                svgStrm.Seek(0, SeekOrigin.Begin);

                var svgDoc = SvgDocument.Open<SvgDocument>(svgStrm);
                var bitmap = svgDoc.Draw();
                bitmap.Save(pngStrm, ImageFormat.Png);

                svgStrm.Close();

                pngStrm.Seek(0, SeekOrigin.Begin);

                var size = PageSizeConverter.ToSize(PageSize.Letter);
                graphics.DrawImage(XImage.FromStream(() => pngStrm), 0, 0, size.Width, size.Height);

                progress.Report(percent);
            }

            document.Save(Path.Combine(outputPath, metadata.VisibleName + ".pdf"));

            return true;
        }

        if (!options.Document.IsT0)
            return false;

        var filename = Path.Combine(_pathManager.NotebooksDir, metadata.ID + ".pdf");
        var doc = options.Document.AsT0;
        var result = new PdfDocument();

        result.Info.Title = metadata.VisibleName;

        for (var i = 0; i < options.PagesIndices.Count; i++)
        {
            var percent = (int)((float)i / (float)options.PagesIndices.Count * 100);
            var rm = metadata.Content.Pages[i];
            var rmPath = Path.Combine(_pathManager.NotebooksDir, metadata.ID, rm + ".rm");

            //todo: figure out index
            PdfPage p = result.AddPage(doc.Pages[i], AnnotationCopyingType.DeepCopy);
            if (!File.Exists(rmPath))
            {
                continue;
            }

            //render
            var notebookStream = File.OpenRead(rmPath);
            var page = Notebook.LoadPage(notebookStream);

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
            var pngImage = XImage.FromStream(() => pngStrm);

            graphics.DrawImage(pngImage, new XPoint(0, 0));

            progress.Report(percent);
        }

        result.Save(Path.Combine(outputPath, result.Info.Title + ".pdf"));

        return true;
    }

    public override string ToString() => Title;
}
