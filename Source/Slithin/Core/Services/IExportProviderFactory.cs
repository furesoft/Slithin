using Slithin.Core.Remarkable;

namespace Slithin.Core.Services;

public interface IExportProviderFactory
{
    IExportProvider[] GetAvailableProviders(Metadata md);

    IExportProvider GetExportProvider(string type);

    void Init();
}