using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Slithin.Core.Services;
using Svg;
using Svg.Pathing;

namespace Slithin.Core.Remarkable.LinesAreBeatiful.Exporters
{
    public static class SvgExporter
    {
        public static string RenderPage(Page page, int index, Metadata md)
        {
            var svgDoc = new SvgDocument
            {
                Width = 1404,
                Height = 1872,
                ViewBox = new SvgViewBox(0, 0, 1404, 1872),
            };

            var group = new SvgGroup();
            svgDoc.Children.Add(group);

            group.Children.Add(new SvgImage { Href = "data:image/png;base64," + GetBase64Template(index, md), X = 0, Y = 0 });

            foreach (var layer in page.Layers)
            {
                foreach (var line in layer.Lines)
                {
                    RenderLine(line, group);
                }
            }

            var stream = new MemoryStream();
            svgDoc.Write(stream);

            return Encoding.UTF8.GetString(stream.GetBuffer());
        }

        private static SvgPathSegmentList GeneratePathData(List<Point> points)
        {
            var psl = new SvgPathSegmentList();
            psl.Add(new SvgMoveToSegment(new PointF(points[0].X, points[0].Y)));

            for (int i = 0; i + 1 < points.Count; i++)
            {
                psl.Add(new SvgLineSegment(
                    new PointF(points[i].X, points[i].Y),
                    new PointF(points[i + 1].X, points[i + 1].Y))
                );

                i++;
            }
            psl.Add(new SvgClosePathSegment());

            return psl;
        }

        private static string GetBase64Template(int i, Metadata md)
        {
            var filename = md.PageData.Data[i];
            var pathManager = ServiceLocator.Container.Resolve<IPathManager>();
            var buffer = File.ReadAllBytes(Path.Combine(pathManager.TemplatesDir, filename + ".png"));

            return Convert.ToBase64String(buffer);
        }

        private static void RenderLine(Line line, SvgGroup group)
        {
            var path = new SvgPath();

            path.PathData = GeneratePathData(line.Points);

            group.Children.Add(path);
        }
    }
}
