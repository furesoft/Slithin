namespace Slithin.ModuleSystem.StdLib;

[WasmExport("std")]
public class Allocator
{
    [WasmImportGlobal("_heap_base")] public static int HeapBaseAddress;

    [WasmExport("malloc")]
    public static int Allocate(int size)
    {
        return new Pointer(0);
    }

    [WasmExport("free")]
    public static void Free(int addr)
    {
        var ptr = new Pointer(addr);
    }

    public static Pointer AllocateString(int length)
    {
        //allocate normal memory
        //register address for automatic scope freeing
        //length is with null termination
        return 0;
    }
}