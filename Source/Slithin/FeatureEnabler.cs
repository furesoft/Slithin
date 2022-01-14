using Slithin.Core.Features;
using Slithin.Core.FeatureToggle;

namespace Slithin;

public class FeatureEnabler
{
    public void Init()
    {
#if DEBUG
        Feature<SharableFeature>.Disable();
        Feature<ExportFeature>.Enable();
        Feature<ExportPdfFeature>.Enable();
        Feature<ExportSvgFeature>.Enable();
        Feature<ExportPngFeature>.Enable();
        Feature<MigrationFeature>.Enable();
#else

#endif
    }
}
