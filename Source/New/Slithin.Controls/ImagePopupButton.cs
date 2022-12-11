using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace Slithin.Controls;

public class ImagePopupButton : ToggleButton
{
    public static StyledProperty<Drawing> ImageProperty =
        AvaloniaProperty.Register<ImageButton, Drawing>("Kind");

    public static StyledProperty<object> PopupContentProperty =
        AvaloniaProperty.Register<ImagePopupButton, object>("PopupContent");

    public static StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<ImagePopupButton, string>("Text");

    public Drawing Image
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
