using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Slithin.Controls;

public class ImageButton : Button
{
    public static StyledProperty<Drawing> ImageProperty =
        AvaloniaProperty.Register<ImageButton, Drawing>("Kind");

    public static StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<ImageButton, string>("Text");

    public Drawing Image
    {
        get => GetValue(ImageProperty);
        set => SetValue(ImageProperty, value);
    }

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
}
