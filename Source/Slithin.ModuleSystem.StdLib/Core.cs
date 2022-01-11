namespace Slithin.ModuleSystem.StdLib;

[WasmExport("core")]
public class Core
{
    private static readonly Random Random = new();

    [WasmExport("rand")]
    public static int Rand()
    {
        return Random.Next();
    }
}