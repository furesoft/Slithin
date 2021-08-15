using System;
using Slithin.Core.Remarkable.Rendering;

namespace Slithin.Core.Remarkable.Rendering
{
    public enum BrushBaseSize
    {
        Small,
        Mid,
        Large
    }

    public struct BaseSizes
    {
        public const float Large = 2.125f;
        public const float Mid = 2.0f;
        public const float Small = 1.875f;

        public static float GetValue(BrushBaseSize size)
        {
            switch (size)
            {
                case BrushBaseSize.Small:
                    return Small;

                case BrushBaseSize.Mid:
                    return Mid;

                case BrushBaseSize.Large:
                    return Large;

                default:
                    return Small;
            }
        }

        public static BrushBaseSize Parse(float value)
        {
            switch (value)
            {
                case Small: return BrushBaseSize.Small;
                case Mid: return BrushBaseSize.Mid;
                case Large: return BrushBaseSize.Large;
            }

            return BrushBaseSize.Small;
        }
    }
}
