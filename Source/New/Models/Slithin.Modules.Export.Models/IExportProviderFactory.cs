using Slithin.Core.Remarkable;
using Slithin.Core.ImportExport;
using Slithin.Core.Remarkable.Models;

namespace Slithin.Core.Services;

public interface IExportProviderFactory
{
    IExportProvider[] GetAvailableProviders(Metadata md);

    IExportProvider GetExportProvider(string type);

    void Init();
}