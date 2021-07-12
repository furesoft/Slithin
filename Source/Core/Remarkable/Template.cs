using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using LiteDB;
using Newtonsoft.Json;
using Slithin.Core.Commands;

namespace Slithin.Core.Remarkable
{
    public class Template : INotifyPropertyChanged
    {
        private IImage? _image;

        public event PropertyChangedEventHandler? PropertyChanged;

        [JsonProperty("categories")]
        public string[]? Categories { get; set; }

        [JsonProperty("filename")]
        public string? Filename { get; set; }

        [JsonProperty("iconCode")]
        public string? IconCode { get; set; }

        [JsonIgnore]
        [BsonIgnore]
        public IImage Image
        {
            get { return _image; }
            set { _image = value; PropertyChanged?.Invoke(this, new(nameof(Image))); }
        }

        [JsonProperty("landscape")]
        public bool Landscape { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonIgnore]
        public bool OnDevice { get; set; }

        [JsonIgnore]
        public bool ToDownload { get; set; }

        public void Load()
        {
            if (Directory.Exists(ServiceLocator.TemplatesDir))
            {
                var path = Path.Combine(ServiceLocator.TemplatesDir, Filename);

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
