using System.IO;

namespace Slithin.Core.ImportProviders
{
    public class PdfImportProvider : IImportProvider
    {
        public bool CanHandle(string filename)
        {
            return Path.GetExtension(filename) == ".pdf";
        }

        public Stream Import(Stream source)
        {
            return source;
        }
    }
}
