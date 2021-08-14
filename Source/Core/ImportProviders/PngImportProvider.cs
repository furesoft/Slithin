using System.IO;

namespace Slithin.Core.ImportProviders
{
    [ImportProviderBaseType(".png")]
    public class PngImportProvider : IImportProvider
    {
        public bool CanHandle(string filename)
        {
            return Path.GetExtension(filename) == ".png";
        }

        public Stream Import(Stream source)
        {
            return source;
        }
    }
}
