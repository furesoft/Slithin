using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace Slithin.Controls
{
    public class ImagePopupButton : ToggleButton
    {
        public static StyledProperty<IImage> ImageProperty = AvaloniaProperty.Register<ImagePopupButton, IImage>("Image");
        public static StyledProperty<object> PopupContentProperty = AvaloniaProperty.Register<ImagePopupButton, object>("PopupContent");
        public static StyledProperty<string> TextProperty = AvaloniaProperty.Register<ImagePopupButton, string>("Text");

        public IImage Image
        {
            get { return GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public object PopupContent
        {
            get { return GetValue(PopupContentProperty); }
            set { SetValue(PopupContentProperty, value); }
        }

        public string Text
        {
            get { return GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
    }
}
