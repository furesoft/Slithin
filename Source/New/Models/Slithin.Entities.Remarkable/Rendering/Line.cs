using System.Collections.Generic;

namespace Slithin.Entities.Remarkable.Rendering;

public struct Line
{
    public BrushBaseSize BrushBaseSize;
    public Brushes BrushType;
    public Colors Color;
    public List<Point> Points;
    public int unknown_line_attribute;
}
