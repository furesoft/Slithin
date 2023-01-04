using Slithin.Entities.Remarkable;
using Slithin.Modules.Export.Models;

namespace Slithin.Core.Services;

public interface IExportProviderFactory
{
    IExportProvider[] GetAvailableProviders(Metadata md);

    IExportProvider GetExportProvider(string type);

    void Init();
}
