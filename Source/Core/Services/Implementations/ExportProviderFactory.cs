using System;
using System.Collections.Generic;
using System.Linq;
using Slithin.Core.Remarkable;

namespace Slithin.Core.Services.Implementations
{
    public class ExportProviderFactory : IExportProviderFactory
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
                .Select(type => (IExportProvider)ServiceLocator.Container.Resolve(type));

            foreach (var provider in providers)
            {
                _providers.Add(provider.Title, provider);
            }
        }
    }
}
