using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace Slithin.Core.Remarkable
{
    public class Template : INotifyPropertyChanged
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("filename")]
        public string? Filename { get; set; }

        [JsonPropertyName("iconCode")]
        public string? IconCode { get; set; }

        [JsonPropertyName("categories")]
        public string[]? Categories { get; set; }

        [JsonPropertyName("landscape")]
        public bool Landscape { get; set; }

        private IImage? _image;

        public event PropertyChangedEventHandler? PropertyChanged;

        public IImage Image
        {
            get { return _image; }
            set { _image = value; PropertyChanged?.Invoke(this, new(nameof(Image))); }
        }


        public void Load()
        {
            if (!Directory.Exists("Templates"))
            {
                Directory.CreateDirectory("Templates");
            }

            var path = Path.Combine(Environment.CurrentDirectory, "Templates", Filename + ".png");

            if (!File.Exists(path))
            {
                var output = File.OpenWrite(path);
                ServiceLocator.Scp.Download(path, output);
                output.Close();
            }

            if (Image is null)
            {
                Image = Bitmap.DecodeToWidth(File.OpenRead(path), 150, Avalonia.Visuals.Media.Imaging.BitmapInterpolationMode.HighQuality);
            }
        }
    }
}