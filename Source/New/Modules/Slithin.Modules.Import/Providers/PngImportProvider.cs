using Slithin.Modules.Import.Models;

namespace Slithin.ImportProviders;

[ImportProviderBaseType(".png")]
public class PngImportProvider : IImportProvider
{
    public bool CanHandle(string filename)
        => Path.GetExtension(filename) == ".png";

    public Stream Import(Stream source)
        => source;
}
