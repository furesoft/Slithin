using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Slithin.Controls
{
    public class ImageButton : Button
    {
        public static StyledProperty<IImage> ImageProperty = AvaloniaProperty.Register<ImageButton, IImage>("Image");
        public static StyledProperty<string> TextProperty = AvaloniaProperty.Register<ImageButton, string>("Text");

        public IImage Image
        {
            get { return GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public string Text
        {
            get { return GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
    }
}
