using System;
using System.Collections.Generic;
using System.IO;
using EpubSharp;
using PdfSharpCore.Drawing;
using Slithin.Core.FeatureToggle;
using Slithin.Core.ImportExport;
using Slithin.Core.Remarkable.Exporting.Rendering;
using Slithin.Core.Remarkable.Models;
using Slithin.Core.Services;
using Slithin.Features;

namespace Slithin.Modules.Export.Exporters;

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
        var doc = options.Document.AsT2;

        for (var pageIndex = 0; pageIndex < doc.Resources.Html.Count; pageIndex++)
        {
            var percent = (int)((float)pageIndex / (float)options.PagesIndices.Count * 100);
            var rm = metadata.Content.Pages[pageIndex];
            var rmPath = Path.Combine(_pathManager.NotebooksDir, metadata.ID, rm + ".rm");

            //ToDo: implement epub export

            var p = ((IReadOnlyList<EpubTextFile>)doc.Resources.Html)[pageIndex];
            if (!File.Exists(rmPath))
            {
                continue;
            }

            //render
            var notebookStream = File.OpenRead(rmPath);
            var page = Notebook.LoadPage(notebookStream);

            var psize = new XSize(1404, 1872);
            var svgStrm = (MemoryStream)SvgRenderer.RenderPage(page, pageIndex, metadata, (int)psize.Width, (int)psize.Height);

            svgStrm.Seek(0, SeekOrigin.Begin);

            doc.Resources.Images.Add(new EpubByteFile()
            {
                Content = svgStrm.ToArray(),
                ContentType = EpubSharp.Format.EpubContentType.ImageSvg,
                FileName = pageIndex + ".svg"
            });

            progress.Report(percent);
        }

        EpubWriter.Write(doc, Path.Combine(outputPath, doc.Title + ".epub"));

        return true;
    }

    public override string ToString()
    {
        return "EPUB Ebook";
    }
}
