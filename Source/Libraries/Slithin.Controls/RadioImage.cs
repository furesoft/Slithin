using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Slithin.Controls;

public class RadioImage : RadioButton
{
    public static readonly StyledProperty<IImage> ImageProperty =
        AvaloniaProperty.Register<RadioImage, IImage>(nameof(Image));

    public IImage Image
    {
        get => GetValue(ImageProperty);
        set => SetValue(ImageProperty, value);
    }
}
