using System;
using System.Linq;
using System.Reflection;

namespace Slithin.Core.FeatureToggle;

public static class Features
{
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
        var types = Assembly.GetExecutingAssembly().GetTypes().Where(_ => _.IsAssignableTo(typeof(IFeature)));

        foreach (var type in types)
        {
            var featureType = typeof(Feature<>).MakeGenericType(new Type[] { type });
            featureType.GetMethod("Enable").Invoke(null, null);
        }
    }
}
