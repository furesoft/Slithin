namespace Slithin.Modules.Export.Models.Rendering;

public struct Page
{
    /**
         * All the layers of this page, from the bottom to the top of the
         * layer stack.
         */
    public List<Layer> Layers;

    /*
    public static Page FromSvg(SvgDocument doc)
    {
        var pag = new Page();

        pag.Layers = new List<Layer>();

        var paths = doc.Children.FindSvgElementsOf<SvgPath>();

        foreach (var path in paths)
        {
            var layer = new Layer();

            var line = new Line();
            line.BrushBaseSize = BrushBaseSize.Mid;
            line.BrushType = Brushes.Brush;
            line.Color = Colors.Black;

            var points = new List<Point>();

            foreach (var p in path.PathData)
            {
                var width = 2;

                if (p is SvgMoveToSegment)
                {
                    width = 0;
                }

                points.Add(new Point() { X = p.Start.X, Y = p.Start.Y, Pressure = 2, Direction = 2, Width = width });
                points.Add(new Point() { X = p.End.X, Y = p.End.Y, Pressure = 2, Direction = 2, Width = width });
            }

            line.Points = points;

            layer.Lines = new List<Line>(new[] { line });

            pag.Layers.Add(layer);
        }

        return pag;
    }

    */
}
