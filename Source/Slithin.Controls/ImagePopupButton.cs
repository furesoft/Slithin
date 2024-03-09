using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace Slithin.Controls;

public class ImagePopupButton : ToggleButton
{
    public static readonly StyledProperty<Geometry> ImageProperty =
        AvaloniaProperty.Register<ImageButton, Geometry>("Kind");

    public static readonly StyledProperty<object> PopupContentProperty =
        AvaloniaProperty.Register<ImagePopupButton, object>("PopupContent");

    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<ImagePopupButton, string>("Text");

    public Geometry Image
    {
        get => GetValue(ImageProperty);
        set => SetValue(ImageProperty, value);
    }

    public object PopupContent
    {
        get => GetValue(PopupContentProperty);
        set => SetValue(PopupContentProperty, value);
    }

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
}
