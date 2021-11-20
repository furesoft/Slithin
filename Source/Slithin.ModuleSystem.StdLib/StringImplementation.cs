namespace Slithin.ModuleSystem.StdLib;

[WasmExport("string")]
public class StringImplementation
{
    [WasmExport("length")]
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

    [WasmExport("replace")]
    public static int Replace(int ptrAddress, int replacementAddress)
    {
        var ptr = new Pointer(ptrAddress);
        var length = Strlen(ptrAddress);
        var str = ptr.ReadString(length);

        var ptrReplacement = new Pointer(ptrAddress);
        var lengthReplacement = Strlen(ptrAddress);
        var strReplacement = ptr.ReadString(length);

        var newPtr = Allocator.AllocateString(length + 1);
        newPtr.Write(str.Replace(str, strReplacement));

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

        var newPtr = (Pointer) Allocator.Allocate(split.Select(_ => _.Length + 1).Sum());
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

    [WasmExport("compare")]
    public static int Compare(int lhsAddress, int rhsAddress)
    {
        var ptr = new Pointer(lhsAddress);
        var length = Strlen(lhsAddress);
        var str = ptr.ReadString(length);

        var ptrRhs = new Pointer(rhsAddress);
        var lengthRhs = Strlen(rhsAddress);
        var strRhs = ptrRhs.ReadString(lengthRhs);

        var result = str.CompareTo(strRhs);

        return result;
    }
}