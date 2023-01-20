using Slithin.Core;
using Slithin.Entities.Remarkable;
using Slithin.Modules.Export.Models;

namespace Slithin.Modules.Export;

public class ExportProviderFactoryImpl : IExportProviderFactory
{
    private readonly Dictionary<string, IExportProvider> _providers = new();

    public IExportProvider[] GetAvailableProviders(Metadata md)
    {
        return _providers.Values.Where(_ => _.CanHandle(md)).ToArray();
    }

    public IExportProvider GetExportProvider(string format)
    {
        return _providers[format];
    }

    public void Init()
    {
        var providers = Utils.Find<IExportProvider>();

        foreach (var provider in providers)
        {
            _providers.Add(provider.Title, provider);
        }
    }
}
