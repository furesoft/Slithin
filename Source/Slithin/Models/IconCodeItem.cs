using Avalonia.Media;
using Avalonia.Media.Imaging;
using Slithin.Models;
using Slithin.Core;

namespace Slithin.Models
{
    public class IconCodeItem
    {
        public IImage Image { get; set; }
        public string Name { get; set; }

        public void Load()
        {
            if (Image is not null)
                return;

            var imageStrm = typeof(IconCodeItem).Assembly.GetManifestResourceStream("Slithin.Resources.IconTiles." + Name + ".png");

            Image = Bitmap.DecodeToWidth(imageStrm, 32, Avalonia.Visuals.Media.Imaging.BitmapInterpolationMode.Default);
        }

        public override string ToString() => Name;
    }
}
