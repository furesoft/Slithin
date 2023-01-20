using Slithin.Modules.Import.Models;

namespace Slithin.Modules.Import.Providers;

[ImportProviderBaseType(".pdf")]
[ImportProviderBaseType(".epub")]
public class NotebookImportProvider : IImportProvider
{
    public bool CanHandle(string filename)
        => Path.GetExtension(filename) is ".pdf" or ".epub";

    public Stream Import(Stream source)
        => source;
}
