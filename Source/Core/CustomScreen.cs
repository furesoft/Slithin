using System.IO;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace Slithin.Core
{
    public class CustomScreen
    {
        public string Filename { get; set; }
        public IImage Image { get; set; }
        public string Title { get; set; }

        public void Load()
        {
            if (Directory.Exists(ServiceLocator.CustomScreensDir))
            {
                var path = Path.Combine(ServiceLocator.CustomScreensDir, Filename);

                if (Image is null)
                {
                    if (File.Exists(path + ".png"))
                    {
                        Image = Bitmap.DecodeToWidth(File.OpenRead(path + ".png"), 150, Avalonia.Visuals.Media.Imaging.BitmapInterpolationMode.HighQuality);
                    }
                }
            }
        }
    }
}
