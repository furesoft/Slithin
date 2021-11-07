using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Slithin.Core.Services;
using Svg;
using Svg.Pathing;

namespace Slithin.Core.Remarkable.Rendering
{
    public static class SvgRenderer
    {
        //ToDo: need to use original size dimension
        public static Stream RenderPage(Page page, int index, Metadata md, int width = 1404, int height = 1872)
        {
            var svgDoc = new SvgDocument
            {
                Width = width,
                Height = height,
                ViewBox = new SvgViewBox(0, 0, 1404, 1872),
            };

            var group = new SvgGroup();
            svgDoc.Children.Add(group);

            var template = GetBase64Template(index, md);

            if (template != null)
            {
                group.Children.Add(new SvgImage { Href = "data:image/png;base64," + template, X = 0, Y = 0 });
            }

            foreach (var layer in page.Layers)
            {
                foreach (var line in layer.Lines)
                {
                    if (line.BrushType != Brushes.Eraseall && line.BrushType != Brushes.Rubber)
                    {
                        RenderLine(line, group);
                    }
                }
            }

            var stream = new MemoryStream();
            svgDoc.Write(stream);

            return stream;
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
            if (md.PageData.Data == null)
            {
                return null;
            }

            var filename = md.PageData.Data[i];
            var pathManager = ServiceLocator.Container.Resolve<IPathManager>();
            var buffer = File.ReadAllBytes(Path.Combine(pathManager.TemplatesDir, filename + ".png"));

            return Convert.ToBase64String(buffer);
        }

        private static void RenderLine(Line line, SvgGroup group)
        {
            var path = new SvgPath();

            path.PathData = GeneratePathData(line.Points);

            path.Stroke
                = line.Color switch
                {
                    Colors.Grey => new SvgColourServer(Color.Gray),
                    Colors.White => new SvgColourServer(Color.White),
                    _ => new SvgColourServer(Color.Black),
                };
            path.StrokeWidth
                = line.BrushType switch
                {
                    Brushes.Highlighter or Brushes.Rubber => new SvgUnit(20 * BaseSizes.GetValue(line.BrushBaseSize)),
                    _ => new SvgUnit(18 * BaseSizes.GetValue(line.BrushBaseSize) - 32),
                };

            if (line.BrushType == Brushes.Highlighter)
            {
                path.Opacity = 0.25f;
                path.Stroke = new SvgColourServer(Color.Yellow);
            }
            path.StrokeLineJoin = SvgStrokeLineJoin.Round;
            path.StrokeLineCap = SvgStrokeLineCap.Round;

            group.Children.Add(path);
        }
    }
}
