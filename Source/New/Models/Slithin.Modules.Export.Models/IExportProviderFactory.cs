using Slithin.Entities.Remarkable;

namespace Slithin.Modules.Export.Models;

public interface IExportProviderFactory
{
    IExportProvider[] GetAvailableProviders(Metadata md);

    IExportProvider GetExportProvider(string type);

    void Init();
}
