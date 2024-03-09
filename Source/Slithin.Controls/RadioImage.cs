using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Slithin.Controls;

public class RadioImage : RadioButton
{
    public static readonly StyledProperty<Geometry> ImageProperty =
        AvaloniaProperty.Register<RadioImage, Geometry>(nameof(Image));

    public Geometry Image
    {
        get => GetValue(ImageProperty);
        set => SetValue(ImageProperty, value);
    }
}
