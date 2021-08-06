using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Avalonia.Media;
using Avalonia.Media.Imaging;

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
            if (Directory.Exists(ServiceLocator.CustomScreensDir))
            {
                var path = Path.Combine(ServiceLocator.CustomScreensDir, Filename);

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
