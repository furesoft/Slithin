using Slithin.Entities.Remarkable;
using Slithin.Entities.Remarkable.Rendering;

namespace Slithin.Modules.Export.Models;

public interface IRenderingService
{
    Stream RenderSvg(Page page, int pageIndex, Metadata md, int width = 1404, int height = 1872);

    Stream RenderPng(Page page, int pageIndex, Metadata md, int width = 1404, int height = 1872);
}
