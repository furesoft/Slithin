using System.IO;

namespace Slithin.Models
{
    public class NotebookCustomPage
    {
        public NotebookCustomPage(string filename, int count)
        {
            Filename = filename;
            Count = count;
            ShortName = Path.GetFileName(filename);
        }

        public int Count { get; set; }
        public string Filename { get; set; }
        public string ShortName { get; set; }
    }
}
