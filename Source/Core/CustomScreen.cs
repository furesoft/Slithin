using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Slithin.Core.Services;

namespace Slithin.Core
{
    public class CustomScreen : INotifyPropertyChanged
    {
        private IImage _image;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Filename { get; set; }

        public IImage Image
        {
            get { return _image; }
            set { _image = value; OnChange(); }
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

        private void OnChange([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
