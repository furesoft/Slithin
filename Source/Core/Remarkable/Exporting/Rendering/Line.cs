using System.Collections.Generic;

namespace Slithin.Core.Remarkable.Rendering
{
    public struct Line
    {
        /**
         * Kind of brush that was selected while drawing the line.
         * @see rmlab::Brushes
         */
        public BrushBaseSize BrushBaseSize;
        public Brushes BrushType;
        public Colors Color;
        public List<Point> Points;
        public int unknown_line_attribute;
        /**
         * Color of the brush as selected while drawing the line.
         * @see rmlab::Colors
         */
        /**
         * Attribute whose purpose is still unknown.
         */
        /**
         * Base size of the brush as selected while drawing the line.
         * @see rmlab::BaseSizes
         */
        /**
         * All points contained in this line, in the order they were drawn.
         */
    }
}
