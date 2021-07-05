using FeatureSwitcher;
using features = FeatureSwitcher.Configuration.Features;

namespace Slithin.Core
{
    public static class FeatureExtensions
    {
        public static void Enable<T>(this T feature)
            where T : IFeature
        {
            features.Are.ConfiguredBy.Custom(features.OfType<T>.Enabled);
        }
    }
}
