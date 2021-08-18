using Avalonia;
using Avalonia.Controls;
using Material.Icons;

namespace Slithin.Controls
{
    public class ImageButton : Button
    {
        public static StyledProperty<MaterialIconKind> KindProperty = AvaloniaProperty.Register<ImageButton, MaterialIconKind>("Kind");
        public static StyledProperty<string> TextProperty = AvaloniaProperty.Register<ImageButton, string>("Text");

        public MaterialIconKind Kind
        {
            get { return GetValue(KindProperty); }
            set { SetValue(KindProperty, value); }
        }

        public string Text
        {
            get { return GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
    }
}
