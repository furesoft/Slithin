using System;
using Avalonia.Markup.Xaml;

namespace Slithin.Core.FeatureToggle;

public class FeatureEnabledExtension : MarkupExtension
{
    public FeatureEnabledExtension(string featureName)
    {
        FeatureName = featureName;
    }

    public string FeatureName { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return Features.FromString(FeatureName).IsEnabled;
    }
}
