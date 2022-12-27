using AuroraModularis.Core;
using SkiaSharp;
using Slithin.Entities.Remarkable;
using Slithin.Entities.Remarkable.Rendering;
using Slithin.Modules.Export.Models;
using Slithin.Modules.Repository.Models;

namespace Slithin.Modules.Export;

internal class RenderingServiceImpl : IRenderingService
{
    public Stream RenderPng(Page page, int pageIndex, Metadata md, int width = 1404, int height = 1872)
    {
        throw new NotImplementedException();
    }

    public Stream RenderSvg(Page page, int pageIndex, Metadata md, int width = 1404, int height = 1872)
    {
        var stream = new MemoryStream();

        using (var wstream = new SKManagedWStream(stream))
        using (var svg = SKSvgCanvas.Create(SKRect.Create(100, 100), wstream))
        {
            var templateFilename = GetTemplateFilename(pageIndex, md);
            var templateImage = SKImage.FromBitmap(SKBitmap.Decode(templateFilename));

            svg.DrawImage(templateImage, SKRect.Create(0, 0, width, height));
        }

        foreach (var layer in page.Layers)
        {
            foreach (var line in layer.Lines)
            {
                if (line is not { BrushType: Brushes.Eraseall } && line.BrushType != Brushes.Rubber)
                {
                    // RenderLine(line, group);
                }
            }
        }

        stream.Seek(0, SeekOrigin.Begin);
        return stream;
    }

    private static string GetTemplateFilename(int i, Metadata md)
    {
        if (md.PageData.Data == null)
        {
            return null;
        }

        var filename = i < md.PageData.Data.Length ? md.PageData.Data[i] : "Blank";
        var pathManager = Container.Current.Resolve<IPathManager>();

        return Path.Combine(pathManager.TemplatesDir, filename + ".png");
    }
}
