using Avalonia.Media;

namespace Slithin.Models
{
    public class Sharable
    {
        public string Author { get; set; }
        public string ID { get; set; }
        public IImage Image { get; set; }
        public bool IsInstalled { get; set; }
        public string Name { get; set; }
    }
}
