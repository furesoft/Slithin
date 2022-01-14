using FeatureSwitcher.Configuration;
using Slithin.Core.Features;

namespace Slithin;

public class FeatureEnabler
{
    public void Init()
    {
#if DEBUG
        Features.Are.ConfiguredBy.Custom(Features.OfType<SharableFeature>.Enabled);
        Features.Are.ConfiguredBy.Custom(Features.OfType<ExportFeature>.Enabled);
        Features.Are.ConfiguredBy.Custom(Features.OfType<ExportPdfFeature>.Enabled);
        Features.Are.ConfiguredBy.Custom(Features.OfType<ExportSvgFeature>.Enabled);
        Features.Are.ConfiguredBy.Custom(Features.OfType<ExportPngFeature>.Enabled);
        Features.Are.ConfiguredBy.Custom(Features.OfType<MigrationFeature>.Enabled);
#else

#endif
    }
}
