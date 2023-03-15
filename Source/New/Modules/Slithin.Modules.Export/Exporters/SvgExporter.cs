using Slithin.Entities.Remarkable;
using Slithin.Modules.Export.Models;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.UI.Models;

namespace Slithin.Modules.Export.Exporters;

public class SvgExporter : IExportProvider
{
    private readonly ILocalisationService _localisationService;
    private readonly IDialogService _dialogService;
    private readonly IRenderingService _renderingService;

    public SvgExporter(ILocalisationService localisationService,
                       IDialogService dialogService,
                       IRenderingService renderingService)
    {
        _localisationService = localisationService;
        _dialogService = dialogService;
        _renderingService = renderingService;
    }

    public bool ExportSingleDocument => false;

    public string Title => "SVG Graphics";

    public bool CanHandle(Metadata md)
    {
        return md.Content.FileType == "notebook";
    }

    public bool Export(ExportOptions options, Metadata metadata, string outputPath, IProgress<int> progress)
    {
        if (!options.Document.IsT1)
        {
            return false;
        }

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

            var svgStrm = _renderingService.RenderSvg(page, i, metadata,options.ShouldHideTemplates);
            var outputStrm = File.Create(Path.Combine(outputPath, $"{i}.svg"));

            svgStrm.CopyTo(outputStrm);

            svgStrm.Close();
            outputStrm.Close();

            progress.Report(percent);
        }

        return true;
    }

    public override string ToString() => Title;
}
