using System;
using Avalonia.Markup.Xaml;

namespace Slithin.Core.FeatureToggle;

public class FeatureEnabledMarkupExtension : MarkupExtension
{
    public FeatureEnabledMarkupExtension(string featureName)
    {
        FeatureName = featureName;
    }

    public string FeatureName { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return Features.FromString(FeatureName).IsEnabled;
    }
}
