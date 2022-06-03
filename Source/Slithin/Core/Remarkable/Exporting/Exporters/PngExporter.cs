using System.Drawing.Imaging;
using System.IO;
using Slithin.Core.ImportExport;
using Slithin.Core.Remarkable.Exporting.Rendering;
using Slithin.Core.Remarkable.Models;
using Svg;
using SvgRenderer = Slithin.Core.Remarkable.Exporting.Rendering.SvgRenderer;

namespace Slithin.Core.Remarkable.Exporting.Exporters;

public class PngExporter : IExportProvider
{
    public bool ExportSingleDocument => false;
    public string Title => "PNG Graphics";

    public bool CanHandle(Metadata md)
    {
        return md.Content.FileType == "notebook";
    }

    public bool Export(ExportOptions options, Metadata metadata, string outputPath)
    {
        if (!options.Document.IsT1)
            return false;

        var notebook = options.Document.AsT1;

        for (var i = 0; i < options.PagesIndices.Count; i++)
        {
            var page = notebook.Pages[options.PagesIndices[i]];

            var svgStrm = SvgRenderer.RenderPage(page, i, metadata);

            var doc = SvgDocument.Open<SvgDocument>(svgStrm);
            var bitmap = doc.Draw();
            bitmap.Save(Path.Combine(outputPath, i + ".png"), ImageFormat.Png);

            svgStrm.Close();
        }

        return true;
    }

    public override string ToString() => Title;
}
