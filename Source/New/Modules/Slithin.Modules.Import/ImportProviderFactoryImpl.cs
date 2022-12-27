using System.Reflection;
using AuroraModularis.Core;
using Slithin.Modules.Import.Models;

namespace Slithin.Core.Services.Implementations;

public class ImportProviderFactoryImpl : IImportProviderFactory
{
    private readonly List<IImportProvider> _providers = new();

    public IImportProvider GetImportProvider(string baseExtension, string filename)
    {
        foreach (var provider in _providers)
        {
            var type = provider.GetType();
            var attrs = type.GetCustomAttributes<ImportProviderBaseTypeAttribute>();

            foreach (var attr in attrs)
            {
                if (attr.Extension != baseExtension)
                {
                    continue;
                }

                if (provider.CanHandle(filename))
                {
                    return provider;
                }
            }
        }

        return null;
    }

    public void Init()
    {
        static Type[] GetTypes(System.Reflection.Assembly s)
        {
            try
            {
                return s.GetTypes();
            }
            catch
            {
                return Array.Empty<Type>();
            }
        }

        var providers = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(GetTypes)
            .Where(x => typeof(IImportProvider).IsAssignableFrom(x) && x.IsClass)
            .Select(type => Container.Current.Resolve<IImportProvider>(type));

        _providers.AddRange(providers);
    }
}
