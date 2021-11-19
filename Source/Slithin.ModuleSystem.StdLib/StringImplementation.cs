namespace Slithin.ModuleSystem.StdLib;

[WasmExport("string")]
public class StringImplementation
{
    [WasmExport("strLen")]
    public static int Strlen(int ptrAddress)
    {
        var ptr = new Pointer(ptrAddress);

        var len = 0;
        while (ptr[len] != 0) len++;

        return len;
    }

    [WasmExport("trim")]
    public static int Trim(int ptrAddress)
    {
        var ptr = new Pointer(ptrAddress);
        var length = Strlen(ptrAddress);
        var str = ptr.ReadString(length);

        var newPtr = Allocator.AllocateString(length + 1);
        newPtr.Write(str.Trim());

        return newPtr;
    }

    [WasmExport("split")]
    public static int Split(int ptrAddress, int seperatorAddress)
    {
        var ptr = new Pointer(ptrAddress);
        var seperatorPtr = new Pointer(seperatorAddress);

        var length = Strlen(ptrAddress);
        var str = ptr.ReadString(length);

        var split = str.Split(seperatorPtr.ReadString(Strlen(seperatorAddress)));

        var newPtr = Allocator.Allocate(split.Select(_ => _.Length + 1).Sum());
        foreach (var s in split)
        {
            newPtr.Write(s);
            newPtr += s.Length + 1;
        }

        return newPtr;
    }

    [WasmExport("substr")]
    public static int Substr(int ptrAddress, int start, int count)
    {
        var ptr = new Pointer(ptrAddress);
        var length = Strlen(ptrAddress);
        var str = ptr.ReadString(length);

        var newPtr = Allocator.AllocateString(count + 2);
        newPtr.Write(str.Substring(start, count));

        return newPtr;
    }
}