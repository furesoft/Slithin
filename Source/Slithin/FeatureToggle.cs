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
            Feature<TemplateEditorFeature>.Enable();
            Feature<SharableFeature>.Enable();
            Feature<ExportFeature>.Enable();
            Feature<ExportPdfFeature>.Enable();
            Feature<ExportSvgFeature>.Enable();
            Feature<ExportPngFeature>.Enable();
        */
#endif
    }
}
