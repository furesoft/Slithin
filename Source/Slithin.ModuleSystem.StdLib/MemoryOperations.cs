namespace Slithin.ModuleSystem.StdLib;

[WasmExport("memory")]
public static class MemoryOperations
{
    [WasmExport("memcpy")]
    public static void Memcpy(int dstAddress, int srcAddress, int length)
    {
        var dst = new Pointer(dstAddress);
        var src = new Pointer(srcAddress);

        for (var i = 0; i < length; i++) dst[i] = src[i];
    }

    [WasmExport("memset")]
    public static void Memset(int ptrAddress, int ch, int length)
    {
        var ptr = new Pointer(ptrAddress);
        for (var i = 0; i < length; i++) ptr[i] = ch;
    }
}