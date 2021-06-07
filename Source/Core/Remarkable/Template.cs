using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

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

        private IImage _image;

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

            if (!File.Exists(Path.Combine("Templates", Filename + ".png")))
            {
                var output = File.OpenWrite(Path.Combine("Templates", Filename + ".png"));
                ServiceLocator.Scp.Download(PathList.Templates + "/" + Filename + ".png", output);
                output.Close();
            }

            Image = new Bitmap(Path.Combine(Environment.CurrentDirectory, "Templates", Filename + ".png"));
        }
    }
}