using Slithin.Entities.Remarkable;
using Slithin.Modules.Export.Models;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.UI.Models;

namespace Slithin.Modules.Export.Exporters;

public class PngExporter : IExportProvider
{
    private readonly ILocalisationService _localisationService;
    private readonly IDialogService _dialogService;
    private readonly IRenderingService _renderingService;

    public PngExporter(ILocalisationService localisationService,
                       IDialogService dialogService,
                       IRenderingService renderingService)
    {
        _localisationService = localisationService;
        _dialogService = dialogService;
        _renderingService = renderingService;
    }

    public bool ExportSingleDocument => false;
    public string Title => "PNG Graphics";

    public bool CanHandle(Metadata md)
    {
        return md.Content.FileType == "notebook";
    }

    public bool Export(ExportOptions options, Metadata metadata, string outputPath, IProgress<int> progress)
    {
        if (!options.Document.IsT1)
            return false;

        var notebook = options.Document.AsT1;

        if (options.PagesIndices.Count == 0)
        {
            _dialogService.Show("No Pages To Export Selected");
            return false;
        }

        for (var i = 0; i < options.PagesIndices.Count; i++)
        {
            var percent = (int)((float)i / (float)options.PagesIndices.Count * 100);

            var page = notebook.Pages[options.PagesIndices[i]];

            var pngStrm = _renderingService.RenderPng(page, i, metadata, options.ShouldHideTemplates);

            using var fileStrm = File.Open(Path.Combine(outputPath, $"{i}.png"), FileMode.OpenOrCreate);
            pngStrm.CopyTo(fileStrm);
            pngStrm.Close();

            progress.Report(percent);
        }

        return true;
    }

    public override string ToString() => Title;
}
