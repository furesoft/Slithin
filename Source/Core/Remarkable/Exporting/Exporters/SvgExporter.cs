using System.IO;

namespace Slithin.Core.Remarkable.Rendering.Exporters
{
    public class SvgExporter : IExportProvider
    {
        public bool ExportSingleDocument => false;
        public string Title => "SVG Graphics";

        public void Export(Notebook notebook, Metadata metadata, string outputPath)
        {
            for (var i = 0; i < notebook.Pages.Count; i++)
            {
                var page = notebook.Pages[i];

                var svgStrm = SvgRenderer.RenderPage(page, i, metadata);
                var outputStrm = File.OpenWrite(Path.Combine(outputPath, i + ".svg"));

                svgStrm.CopyTo(outputStrm);

                svgStrm.Close();
                outputStrm.Close();
            }
        }
    }
}
