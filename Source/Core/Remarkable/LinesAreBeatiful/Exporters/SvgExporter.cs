using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slithin.Core.Remarkable.LinesAreBeatiful.Exporters
{
    public static class SvgExporter
    {
        public static string RenderPage(Page page)
        {
            StringBuilder sb = new();
            // SVG header
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\" ?>");
            sb.AppendLine("<svg width=\"1404\" height=\"1872\" viewBox=\"0 0 1404 1872\" ");
            sb.AppendLine("version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" ");
            sb.AppendLine("xmlns:xlink=\"http://www.w3.org/1999/xlink\">");

            for (var i = 0; i < page.Layers.Count; i++)
            {
                var layer = page.Layers[i];

                RenderLayer(i, layer, sb);
            }

            // SVG footer
            sb.AppendLine("</svg>");

            return sb.ToString();
        }

        private static void RenderLayer(int layer_id, Layer layer, StringBuilder sb)
        {
            var layer_id_str = "layer-" + layer_id;
            Stack<RenderGroup> open_groups = new();
            RenderGroup current = new();

            foreach (var line in layer.Lines)
            {
                switch (line.BrushType)
                {
                    case Brushes.Rubber:
                    case Brushes.Rubberarea:
                        if (current.Strokes.Any())
                        {
                            // Register an erasure only if there exists previous
                            // strokes to erase (otherwise, there is nothing to
                            // erase and it can be safely ignored).
                            current.Erasures.Add(line);
                        }

                        break;

                    default:
                        if (current.Erasures.Any())
                        {
                            // We cannot add a new stroke to a group that already contains
                            // some erasures, because this new stroke should not be
                            // affected by the previous erasures; so, we create a new group.
                            open_groups.Push(current);
                            current = new();
                        }

                        current.Strokes.Add(line);
                        break;
                }
            }

            open_groups.Push(current);

            // (Second pass.) Pop out groups from the stack, thereby creating
            // masks and opening the SVG groups. Prepare a reversed stack to
            // later close the groups.
            Stack<RenderGroup> close_groups = new();
            uint mask_id = 0u;

            while (open_groups.Any())
            {
                var group = open_groups.Pop();

                // Create a mask group for all erasures of the group, if any
                if (group.Erasures.Any())
                {
                    var mask_id_str = layer_id_str + "-mask-" + mask_id;

                    sb.AppendLine("<mask id=\"" + mask_id_str + "\">");
                    sb.AppendLine("<rect width=\"100%\" height=\"100%\" fill=\"white\" />");

                    foreach (var erasure in group.Erasures)
                    {
                        RenderLine(erasure, sb);
                    }

                    sb.AppendLine("</mask>");
                    sb.AppendLine("<g mask=\"url(#" + mask_id_str + ")\" ");
                }
                else
                {
                    sb.AppendLine("<g ");
                }

                if (mask_id == 0)
                {
                    sb.AppendLine("id=\"" + layer_id_str + "\" ");
                }

                sb.AppendLine(">");

                close_groups.Push(group);

                ++mask_id;
            }

            // (Third pass.) Generate paths for each stroke of each group and
            // then close the groups.
            while (close_groups.Any())
            {
                var cgroup = close_groups.Pop();
                foreach (var stroke in cgroup.Strokes)
                {
                    RenderLine(stroke, sb);
                }

                sb.AppendLine("</g>");
            }
        }

        private static void RenderLine(Line line, StringBuilder sb)
        {
            switch (line.BrushType)
            {
                case Brushes.Rubberarea:
                    RenderRubberArea(line, sb);
                    break;

                default:
                    RenderNormalLine(line, sb);
                    break;
            }
        }

        private static void RenderNormalLine(Line line, StringBuilder sb)
        {
            // TODO: apply brush texture, pressure and tilt parameters

            sb.AppendLine("<path ");
            sb.AppendLine("fill=\"none\" ");
            sb.AppendLine("stroke=\"");

            switch (line.Color)
            {
                case Colors.Grey:
                    sb.AppendLine("grey");
                    break;

                case Colors.White:
                    sb.AppendLine("white");
                    break;

                default:
                    sb.AppendLine("black");
                    break;
            }

            sb.AppendLine("\" ");
            sb.AppendLine("stroke-width=\"");

            switch (line.BrushType)
            {
                case Brushes.Highlighter:
                case Brushes.Rubber:
                    sb.AppendLine((20 * BaseSizes.GetValue(line.BrushBaseSize)).ToString());
                    break;

                default:
                    sb.AppendLine((18 * BaseSizes.GetValue(line.BrushBaseSize) - 32).ToString());
                    break;
            }

            sb.AppendLine("\" ");

            sb.AppendLine("d=\"");
            RenderPathData(line.Points, sb);
            sb.AppendLine("\" ");

            if (line.BrushType == Brushes.Highlighter)
            {
                sb.AppendLine("opacity=\"0.25\" ");
            }

            sb.AppendLine("stroke-linejoin=\"round\" ");
            sb.AppendLine("stroke-linecap=\"round\" ");
            sb.AppendLine("/>");
        }

        private static void RenderPathData(List<Point> points, StringBuilder sb)
        {
            bool is_first = true;

            foreach (var point in points)
            {
                sb.AppendLine(is_first ? "M" : "L");
                sb.AppendLine(point.X + "," + point.Y);

                is_first = false;
            }
        }

        private static void RenderRubberArea(Line line, StringBuilder sb)
        {
            sb.AppendLine("<path ");
            sb.AppendLine("fill=\"black\" ");
            sb.AppendLine("stroke=\"none\" ");
            sb.AppendLine("d=\"");
            RenderPathData(line.Points, sb);
            sb.AppendLine("\" />");
        }

        private class RenderGroup
        {
            public List<Line> Erasures = new();
            public List<Line> Strokes = new();
        }
    }
}
