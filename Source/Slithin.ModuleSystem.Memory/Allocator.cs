namespace Slithin.ModuleSystem.StdLib;

//ToDo: Solve Allocator Problem: Every Module Should Have A Unique Allocator
public class Allocator
{
    public static int HeapBaseAddress;


    public static int Allocate(int size)
    {
        return 0;
    }

    public static void Free(int addr)
    {
    }

    public static int AllocateString(int length)
    {
        //allocate normal memory
        //register address for automatic scope freeing
        //length is with null termination
        return 0;
    }
}
