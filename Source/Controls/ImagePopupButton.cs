using Avalonia;
using Avalonia.Controls.Primitives;
using Material.Icons;

namespace Slithin.Controls
{
    public class ImagePopupButton : ToggleButton
    {
        public static StyledProperty<MaterialIconKind> KindProperty =
            AvaloniaProperty.Register<ImageButton, MaterialIconKind>("Kind");
        public static StyledProperty<object> PopupContentProperty =
            AvaloniaProperty.Register<ImagePopupButton, object>("PopupContent");
        public static StyledProperty<string> TextProperty =
            AvaloniaProperty.Register<ImagePopupButton, string>("Text");

        public MaterialIconKind Kind
        {
            get => GetValue(KindProperty);
            set => SetValue(KindProperty, value);
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
}
