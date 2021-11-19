namespace Slithin.ModuleSystem.StdLib;

public class Allocator
{
    public static Pointer Allocate(int size)
    {
        return default;
    }

    public static void Free(Pointer ptr)
    {
    }

    public static Pointer AllocateString(int length)
    {
        //allocate normal memory
        //register address for automatic scope freeing
        return 0;
    }
}