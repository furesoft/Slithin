using System.IO;
using Slithin.Core.ImportExport;

namespace Slithin.ImportProviders;

[ImportProviderBaseType(".pdf")]
[ImportProviderBaseType(".epub")]
public class NotebookImportProvider : IImportProvider
{
    public bool CanHandle(string filename)
        => Path.GetExtension(filename) is ".pdf" or ".epub";

    public Stream Import(Stream source)
        => source;
}
