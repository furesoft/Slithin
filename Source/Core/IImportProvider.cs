using System.IO;

namespace Slithin.Core
{
    public interface IImportProvider
    {
        bool CanHandle(string filename);

        Stream Import(Stream source);
    }
}
