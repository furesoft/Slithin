using System.IO;
using Slithin.Core;

namespace Slithin.Core.ImportExport;

public interface IImportProvider
{
    bool CanHandle(string filename);

    Stream Import(Stream source);
}
