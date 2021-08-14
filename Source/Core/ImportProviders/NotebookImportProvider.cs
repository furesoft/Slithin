using System.IO;

namespace Slithin.Core.ImportProviders
{
    [ImportProviderBaseType(".pdf")]
    [ImportProviderBaseType(".epub")]
    public class NotebookImportProvider : IImportProvider
    {
        public bool CanHandle(string filename)
        {
            return Path.GetExtension(filename) == ".pdf" || Path.GetExtension(filename) == ".epub";
        }

        public Stream Import(Stream source)
        {
            return source;
        }
    }
}
