namespace Slithin.ModuleSystem.StdLib;

[WasmExport("memory")]
public static class MemoryOperations
{
    [WasmExport("memcpy")]
    public static void Memcpy(Pointer dst, Pointer src, int length)
    {
        for (var i = 0; i < length; i++) dst[i] = src[i];
    }

    [WasmExport("memset")]
    public static void Memset(Pointer ptr, int ch, int length)
    {
        for (var i = 0; i < length; i++) ptr[i] = ch;
    }
}