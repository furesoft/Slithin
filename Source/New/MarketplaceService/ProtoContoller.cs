using System.Reflection;

namespace MarketplaceService;

public class ProtoController
{
    public Stream Get(string protoName)
    {
        var assembly = Assembly.GetEntryAssembly();

        return assembly.GetManifestResourceStream($"{assembly.GetName().Name}.Protos.{protoName}.proto");
    }

    public IEnumerable<string> GetAvailable()
    {
        return Assembly.GetEntryAssembly().GetManifestResourceNames().Select(_ => _.Split('.')[^2]);
    }
}
