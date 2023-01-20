using System.Reflection;

namespace Slithin.Core.FeatureToggle;

public static class Features
{
    private static readonly Dictionary<string, Type> _allFeatures = new();

    public static void Collect()
    {
        foreach (var type in Utils.FindTypes<IFeature>())
        {
            if (!type.IsInterface && !type.IsAbstract && !_allFeatures.ContainsKey(type.Name))
            {
                _allFeatures.Add(type.Name, type);
            }
        }
    }

    public static void Disable<T1, T2, T3>()
        where T1 : IFeature
        where T2 : IFeature
        where T3 : IFeature
    {
        Feature<T1>.Disable();
        Feature<T2>.Disable();
        Feature<T3>.Disable();
    }

    public static void Enable<T1, T2, T3>()
        where T1 : IFeature
        where T2 : IFeature
        where T3 : IFeature
    {
        Feature<T1>.Enable();
        Feature<T2>.Enable();
        Feature<T3>.Enable();
    }

    public static void EnableAll()
    {
#if DEBUG
        foreach (var feature in _allFeatures)
        {
            var dynamicFeature = FromString(feature.Key);
            dynamicFeature.IsEnabled = true;
        }
#endif
    }

    public static DynamicFeature FromString(string featureName)
    {
        return new DynamicFeature(typeof(Feature<>).MakeGenericType(_allFeatures[featureName]));
    }

    public class DynamicFeature
    {
        internal DynamicFeature(Type featureType)
        {
            Property = featureType.GetProperty("IsEnabled");
        }

        public bool IsEnabled
        {
            get => (bool)Property.GetValue(null);
            set => Property.SetValue(null, value);
        }

        private PropertyInfo Property { get; }
    }
}
