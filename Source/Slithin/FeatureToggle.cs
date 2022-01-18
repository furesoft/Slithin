using Slithin.Core.FeatureToggle;

namespace Slithin;

public class FeatureToggle
{
    public void Init()
    {
        Features.Collect();
#if DEBUG

        Features.EnableAll();
#else

        /*
            Feature<SharableFeature>.Enable();
            Feature<ExportFeature>.Enable();
            Feature<ExportPdfFeature>.Enable();
            Feature<ExportSvgFeature>.Enable();
            Feature<ExportPngFeature>.Enable();
            Feature<MigrationFeature>.Enable();
            */
#endif
    }
}
