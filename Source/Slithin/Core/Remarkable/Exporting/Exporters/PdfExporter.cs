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
            return ExportNotebook(options, metadata, outputPath, progress);
        }

        if (!options.Document.IsT0)
            return false;

        return ExportPDF(options, metadata, outputPath, progress);
    }

    public override string ToString() => Title;

    private static bool ExportNotebook(ExportOptions options, Metadata metadata, string outputPath, IProgress<int> progress)
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

            var size = PageSizeConverter.ToSize(PageSize.Letter);
            var pngStrm = RenderSVGAsPng(metadata, i, page, ref size);

            graphics.DrawImage(XImage.FromStream(() => pngStrm), 0, 0, size.Width, size.Height);

            progress.Report(percent);
        }

        document.Save(Path.Combine(outputPath, metadata.VisibleName + ".pdf"));

        return true;
    }

    private static MemoryStream RenderSVGAsPng(Metadata metadata, int pageIndex, Page page, ref XSize psize)
    {
        var svgStrm = SvgRenderer.RenderPage(page, pageIndex, metadata, (int)psize.Width, (int)psize.Height);
        var pngStrm = new MemoryStream();

        svgStrm.Seek(0, SeekOrigin.Begin);

        var d = SvgDocument.Open<SvgDocument>(svgStrm);
        var bitmap = d.Draw();
        bitmap.Save(pngStrm, ImageFormat.Png);
        pngStrm.Seek(0, SeekOrigin.Begin);

        svgStrm.Close();
        return pngStrm;
    }

    private bool ExportPDF(ExportOptions options, Metadata metadata, string outputPath, IProgress<int> progress)
    {
        var filename = Path.Combine(_pathManager.NotebooksDir, metadata.ID + ".pdf");
        var doc = options.Document.AsT0;
        var result = new PdfDocument();

        result.Info.Title = metadata.VisibleName;

        for (var i = 0; i < options.PagesIndices.Count; i++)
        {
            var percent = (int)((float)i / (float)options.PagesIndices.Count * 100);
            var rm = metadata.Content.Pages[i];
            var rmPath = Path.Combine(_pathManager.NotebooksDir, metadata.ID, rm + ".rm");

            PdfPage p = doc.Pages[i];
            if (!File.Exists(rmPath))
            {
                continue;
            }

            //render
            var notebookStream = File.OpenRead(rmPath);
            var page = Notebook.LoadPage(notebookStream);

            var psize = new XSize(1404, 1872);
            var pngStrm = RenderSVGAsPng(metadata, i, page, ref psize);

            var graphics = XGraphics.FromPdfPage(p);

            var pngImage = XImage.FromStream(() => pngStrm);

            graphics.DrawImage(pngImage, 0, 0, p.Width, p.Height);

            progress.Report(percent);
        }

        doc.Save(Path.Combine(outputPath, result.Info.Title + ".pdf"));

        return true;
    }
}
