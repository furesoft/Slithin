using Slithin.Modules.Import.Models;

namespace Slithin.Modules.Import.Providers;

[ImportProviderBaseType(".png")]
public class PngImportProvider : IImportProvider
{
    public bool CanHandle(string filename)
        => Path.GetExtension(filename) == ".png";

    public Stream Import(Stream source)
        => source;
}
