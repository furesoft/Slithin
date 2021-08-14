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
            sb.Append("version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" ");
            sb.Append("xmlns:xlink=\"http://www.w3.org/1999/xlink\">");

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

                    sb.AppendLine("\t<mask id=\"" + mask_id_str + "\">");
                    sb.Append("\t\t<rect width=\"100%\" height=\"100%\" fill=\"white\" />");

                    foreach (var erasure in group.Erasures)
                    {
                        RenderLine(erasure, sb);
                    }

                    sb.AppendLine("\t</mask>");
                    sb.AppendLine("\t<g mask=\"url(#" + mask_id_str + ")\" ");
                }
                else
                {
                    sb.AppendLine("\t<g ");
                }

                if (mask_id == 0)
                {
                    sb.Append("id=\"" + layer_id_str + "\" ");
                }

                sb.Append(">");

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

                sb.AppendLine("\t</g>");
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

            sb.AppendLine("\t<path ");
            sb.Append("fill=\"none\" ");
            sb.Append("stroke=\"");

            switch (line.Color)
            {
                case Colors.Grey:
                    sb.Append("grey");
                    break;

                case Colors.White:
                    sb.Append("white");
                    break;

                default:
                    sb.Append("black");
                    break;
            }

            sb.Append("\" ");
            sb.Append("stroke-width=\"");

            switch (line.BrushType)
            {
                case Brushes.Highlighter:
                case Brushes.Rubber:
                    sb.Append(20 * BaseSizes.GetValue(line.BrushBaseSize));
                    break;

                default:
                    sb.Append(18 * BaseSizes.GetValue(line.BrushBaseSize) - 32);
                    break;
            }

            sb.Append("\" ");

            sb.Append("d=\"");
            RenderPathData(line.Points, sb);
            sb.Append("\" ");

            if (line.BrushType == Brushes.Highlighter)
            {
                sb.Append("opacity=\"0.25\" ");
            }

            sb.Append("stroke-linejoin=\"round\" ");
            sb.Append("stroke-linecap=\"round\" ");
            sb.Append("/>");
        }

        private static void RenderPathData(List<Point> points, StringBuilder sb)
        {
            bool is_first = true;

            foreach (var point in points)
            {
                sb.Append(is_first ? "M" : "L");
                sb.Append(point.X + "," + point.Y);

                is_first = false;
            }
        }

        private static void RenderRubberArea(Line line, StringBuilder sb)
        {
            sb.AppendLine("\t<path ");
            sb.Append("fill=\"black\" ");
            sb.Append("stroke=\"none\" ");
            sb.Append("d=\"");
            RenderPathData(line.Points, sb);
            sb.Append("\" />");
        }

        private class RenderGroup
        {
            public List<Line> Erasures = new();
            public List<Line> Strokes = new();
        }
    }
}
