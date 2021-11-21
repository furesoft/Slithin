namespace Slithin.ModuleSystem.StdLib;

[WasmExport("std")]
public class AllocatorImplementation
{
    [WasmImportGlobal("_heap_base")] public static int HeapBaseAddress;

    [WasmExport("malloc")]
    public static int Allocate(int size)
    {
        return Allocator.Allocate(size);
    }

    [WasmExport("free")]
    public static void Free(int addr)
    {
        Allocator.Free(addr);
    }

    public static Pointer AllocateString(int length)
    {
        return Allocator.AllocateString(length);
    }
}