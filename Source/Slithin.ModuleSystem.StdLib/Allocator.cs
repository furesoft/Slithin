namespace Slithin.ModuleSystem.StdLib;

[WasmExport("std")]
public class Allocator
{
    [WasmExport("malloc")]
    public static Pointer Allocate(int size)
    {
        return default;
    }

    [WasmExport("free")]
    public static void Free(Pointer ptr)
    {
    }

    public static Pointer AllocateString(int length)
    {
        //allocate normal memory
        //register address for automatic scope freeing
        //length is with null termination
        return 0;
    }
}