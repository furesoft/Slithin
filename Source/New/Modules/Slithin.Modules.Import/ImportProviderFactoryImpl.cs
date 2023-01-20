using System.Reflection;
using Slithin.Modules.Import.Models;
using Utils = Slithin.Core.Utils;

namespace Slithin.Modules.Import;

public class ImportProviderFactoryImpl : IImportProviderFactory
{
    private readonly List<IImportProvider> _providers = new();

    public IImportProvider? GetImportProvider(string baseExtension, string filename)
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

        IEnumerable<IImportProvider?> providers = Utils.Find<IImportProvider>();

        _providers.AddRange(providers);
    }
}
