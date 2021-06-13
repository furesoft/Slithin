using System.Reflection;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace Slithin.Core
{
    public class IconCodeItem
    {
        public string Name { get; set; }
        public IImage Image { get; set; }

        public override string ToString() => Name;

        public void Load()
        {
            if (Image is null)
            {
                var imageStrm = typeof(IconCodeItem).Assembly.GetManifestResourceStream("Slithin.Resources.IconTiles." + Name + ".png");

                Image = Bitmap.DecodeToWidth(imageStrm, 32, Avalonia.Visuals.Media.Imaging.BitmapInterpolationMode.Default);
            }
        }
    }
}