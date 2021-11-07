namespace Slithin.Core.Remarkable.Rendering
{
    public struct Point
    {
        /**
         * Position on the X-axis, relative to the upper-left corner
         * of the device’s screen. This value is expressed in pixels
         * and is comprised between rmlab::ranges::Coords::minX and
         * rmlab::ranges::Coords::maxX.
         */
        public float Direction;
        public float Pressure;
        public float Speed;
        public float Width;
        public float X;

        /**
         * Position on the Y-axis, relative to the upper-left corner
         * of the device’s screen. This value is expressed in pixels
         * and is comprised between rmlab::ranges::Coords::minY and
         * rmlab::ranges::Coords::maxY.
         */
        public float Y;

        /**
         * Speed
         */
        /**
         * Direction
         * Range likely between rmlab::ranges::Coords::minDir and
         * rmlab::ranges::Coords::maxDir.
         */
        /**
         * Width
         */
        /**
         * Pressure that was being applied on the screen with the pen when
         * this point was sampled. This value is comprised between
         * rmlab::ranges::minP and rmlab::ranges::maxP.
         */
    }
}
