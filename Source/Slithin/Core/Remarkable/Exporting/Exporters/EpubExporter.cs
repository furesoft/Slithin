using System;
using System.IO;
using Slithin.Core.FeatureToggle;
using Slithin.Core.ImportExport;
using Slithin.Core.Remarkable.Exporting.Rendering;
using Slithin.Core.Remarkable.Models;
using Slithin.Core.Services;
using Slithin.Features;

namespace Slithin.Core.Remarkable.Exporting.Exporters;

public class EpubExporter : IExportProvider
{
    private readonly IPathManager _pathManager;

    public EpubExporter(IPathManager pathManager)
    {
        _pathManager = pathManager;
    }

    public bool ExportSingleDocument => true;

    public string Title => "EPUB Ebook";

    public bool CanHandle(Metadata md)
    {
        return Feature<ExportEpubFeature>.IsEnabled
            && md.Content.FileType == "epub";
    }

    public bool Export(ExportOptions options, Metadata metadata, string outputPath, IProgress<int> progress)
    {
        var filename = Path.Combine(_pathManager.NotebooksDir, metadata.ID + ".epub");
        var doc = options.Document.AsT2;

        for (var i = 0; i < options.PagesIndices.Count; i++)
        {
            var pageIndex = options.PagesIndices[i];
            var percent = (int)((float)i / (float)options.PagesIndices.Count * 100);
            var rm = metadata.Content.Pages[pageIndex];
            var rmPath = Path.Combine(_pathManager.NotebooksDir, metadata.ID, rm + ".rm");

            //ToDo: implement epub export

            progress.Report(percent);
        }

        return true;
    }

    public override string ToString()
    {
        return "EPUB Ebook";
    }
}
