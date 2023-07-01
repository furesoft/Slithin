using PdfSharpCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using Slithin.Entities.Remarkable;
using Slithin.Entities.Remarkable.Rendering;
using Slithin.Modules.BaseServices.Models;
using Slithin.Modules.Export.Models;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.UI.Models;

namespace Slithin.Modules.Export.Exporters;

public class PdfExporter : IExportProvider
{
    private readonly IRenderingService _renderingService;
    private readonly IDialogService _dialogService;
    private readonly IPathManager _pathManager;

    public PdfExporter(IPathManager pathManager,
        IRenderingService renderingService,
        IDialogService dialogService)
    {
        _pathManager = pathManager;
        _renderingService = renderingService;
        _dialogService = dialogService;
    }

    public bool ExportSingleDocument => true;
    public string Title => "PDF Document";

    public bool CanHandle(Metadata md)
    {
        return md.Content.FileType == "notebook" || md.Content.FileType == "pdf";
    }

    public bool Export(ExportOptions options, Metadata metadata, string outputPath, IProgress<int> progress)
    {
        if (options.Document.IsT1)
        {
            return ExportNotebook(options, metadata, outputPath, options.ShouldHideTemplates, progress);
        }

        if (!options.Document.IsT0)
            return false;

        return ExportPDF(options, metadata, outputPath, options.ShouldHideTemplates, progress);
    }

    public override string ToString() => Title;

    private bool ExportNotebook(ExportOptions options, Metadata metadata, string outputPath, bool shouldHideTemplate,
        IProgress<int> progress)
    {
        var notebook = options.Document.AsT1;

        var document = new PdfDocument();

        document.Info.Title = metadata.VisibleName;

        if (options.PagesIndices.Count == 0)
        {
            _dialogService.Show("No Pages To Export Selected");
            return false;
        }

        for (var i = 0; i < options.PagesIndices.Count; i++)
        {
            var pageIndex = options.PagesIndices[i];
            var percent = (int)((float)i / (float)options.PagesIndices.Count * 100);

            var pdfPage = document.AddPage();
            pdfPage.Size = PageSize.Letter;

            var graphics = XGraphics.FromPdfPage(pdfPage);

            var page = notebook.Pages[pageIndex];

            var pngStrm = _renderingService.RenderPng(page, i, metadata);

            if (!shouldHideTemplate)
            {
                DrawTemplate(metadata, graphics, i);
            }

            graphics.DrawImage(XImage.FromStream(() => pngStrm), 0, 0, pdfPage.Width, pdfPage.Height);

            progress.Report(percent);
        }

        document.Save(Path.Combine(outputPath, $"{metadata.VisibleName}.pdf"));

        return true;
    }

    private void DrawTemplate(Metadata metadata, XGraphics graphics, int index)
    {
        if (metadata.PageData.Data == null) return;

        var templateBackgroundFilename = metadata.PageData.Data[index];
        var templatePath = Path.Combine(_pathManager.TemplatesDir, $"{templateBackgroundFilename}.png");

        if (File.Exists(templatePath))
        {
            graphics.DrawImage(XImage.FromFile(templatePath), new XPoint(0, 0));
        }
    }

    private bool ExportPDF(ExportOptions options, Metadata metadata, string outputPath, bool shouldHideTemplates,
        IProgress<int> progress)
    {
        var doc = options.Document.AsT0;

        doc.Info.Title = metadata.VisibleName;

        if (options.PagesIndices.Count == 0)
        {
            _dialogService.Show("No Pages To Export Selected");
            return false;
        }

        for (var i = 0; i < options.PagesIndices.Count; i++)
        {
            var pageIndex = options.PagesIndices[i];
            var percent = (int)((float)i / (float)options.PagesIndices.Count * 100);
            var rm = metadata.Content.Pages[pageIndex];
            var rmPath = Path.Combine(_pathManager.NotebooksDir, metadata.ID, $"{rm}.rm");

            PdfPage p = doc.Pages[pageIndex];
            if (!File.Exists(rmPath))
            {
                continue;
            }

            //render
            var notebookStream = File.OpenRead(rmPath);
            var page = Notebook.LoadPage(notebookStream);

            var pngStrm = _renderingService.RenderPng(page, i, metadata, shouldHideTemplates);

            var graphics = XGraphics.FromPdfPage(p);

            var pngImage = XImage.FromStream(() => pngStrm);

            graphics.DrawImage(pngImage, 0, 0, p.Width, p.Height);

            progress.Report(percent);
        }

        doc.Save(Path.Combine(outputPath, $"{doc.Info.Title}.pdf"));

        return true;
    }
}
