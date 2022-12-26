using System.IO;

namespace Slithin.Modules.Import.Models;

public interface IImportProvider
{
    bool CanHandle(string filename);

    Stream Import(Stream source);
}
