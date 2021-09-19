using System.IO;

namespace Slithin.Core.ImportProviders
{
    [ImportProviderBaseType(".png")]
    public class PngImportProvider : IImportProvider
    {
        public bool CanHandle(string filename)
            => Path.GetExtension(filename) == ".png";

        public Stream Import(Stream source)
            => source;
    }
}
