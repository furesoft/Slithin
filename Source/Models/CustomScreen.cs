using System.IO;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Slithin.Core;
using Slithin.Core.Services;

namespace Slithin.Models
{
    public class CustomScreen : NotifyObject
    {
        private IImage _image;

        public string Filename { get; set; }

        public IImage Image
        {
            get => _image;
            set => SetValue(ref _image, value);
        }

        public string Title { get; set; }

        public void Load()
        {
            var pathManager = ServiceLocator.Container.Resolve<IPathManager>();

            if (!Directory.Exists(pathManager.CustomScreensDir))
                return;

            var path = Path.Combine(pathManager.CustomScreensDir, Filename);

            if (!File.Exists(path))
                return;

            using var strm = File.OpenRead(path);
            Image = Bitmap.DecodeToWidth(strm, 150, Avalonia.Visuals.Media.Imaging.BitmapInterpolationMode.HighQuality);
        }
    }
}
