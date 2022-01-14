namespace Slithin.Core.FeatureToggle;

public static class Feature<T>
    where T : IFeature
{
    public static bool IsEnabled { get; private set; }

    public static void Disable()
    {
        IsEnabled = false;
    }

    public static void Enable()
    {
        IsEnabled = true;
    }
}
