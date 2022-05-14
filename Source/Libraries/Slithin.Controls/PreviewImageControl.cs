using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace Slithin.Controls;

public class PreviewImageControl : TemplatedControl
{
    public static readonly StyledProperty<double> PreviewMaxWidthProperty =
                AvaloniaProperty.Register<PreviewImageControl, double>(nameof(PreviewMaxWidth), 350);

    public static readonly StyledProperty<IImage> SourceProperty =
                        AvaloniaProperty.Register<PreviewImageControl, IImage>(nameof(Source));

    public double PreviewMaxWidth
    {
        get => GetValue(PreviewMaxWidthProperty);
        set => SetValue(PreviewMaxWidthProperty, value);
    }

    public IImage Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }
}
