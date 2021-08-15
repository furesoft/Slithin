using System;
using System.Collections.Generic;
using System.Linq;

namespace Slithin.Core.Services.Implementations
{
    public class ExportProviderFactory : IExportProviderFactory
    {
        private readonly Dictionary<string, IExportProvider> _providers = new();

        public string[] GetAvailableExtensions()
        {
            return _providers.Keys.ToArray();
        }

        public IExportProvider GetExportProvider(string extension)
        {
            return _providers[extension];
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
