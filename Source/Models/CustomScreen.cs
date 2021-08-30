using System.IO;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Slithin.Core.Services;
using Slithin.Core;

namespace Slithin.Models
{
    public class CustomScreen : ReactiveObject
    {
        private IImage _image;

        public string Filename { get; set; }

        public IImage Image
        {
            get { return _image; }
            set { SetValue(ref _image, value); }
        }

        public string Title { get; set; }

        public void Load()
        {
            var pathManager = ServiceLocator.Container.Resolve<IPathManager>();

            if (Directory.Exists(pathManager.CustomScreensDir))
            {
                var path = Path.Combine(pathManager.CustomScreensDir, Filename);

                if (File.Exists(path))
                {
                    using var strm = File.OpenRead(path);
                    Image = Bitmap.DecodeToWidth(strm, 150, Avalonia.Visuals.Media.Imaging.BitmapInterpolationMode.HighQuality);
                }
            }
        }
    }
}
