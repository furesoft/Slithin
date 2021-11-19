namespace Slithin.ModuleSystem.StdLib;

[WasmExport("string")]
public class StringImplementation
{
    [WasmExport("strLen")]
    public static int strlen(Pointer ptr)
    {
        var len = 0;
        while (ptr[len] != 0) len++;

        return len;
    }
}