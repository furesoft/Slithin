using AuroraModularis.Core;
using Slithin.Core.Services;
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
        var providers = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(x => typeof(IExportProvider).IsAssignableFrom(x) && x.IsClass)
            .Select(type => Container.Current.Resolve<IExportProvider>(type));

        foreach (var provider in providers)
        {
            _providers.Add(provider.Title, provider);
        }
    }
}
