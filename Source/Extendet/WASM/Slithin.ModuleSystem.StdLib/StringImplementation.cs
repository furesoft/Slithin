namespace Slithin.ModuleSystem.StdLib;

[WasmExport("string")]
public class StringImplementation
{
    [WasmExport("length")]
    public static int Length(int ptrAddress)
    {
        var ptr = new Pointer(ptrAddress);

        var len = 0;
        while (ptr[len] != 0) len++;

        return len;
    }

    [WasmExport("trim")]
    public static int Trim(int ptrAddress)
    {
        var str = Utils.StringFromPtr(ptrAddress);

        var value = str.Trim();

        var newPtr = AllocatorImplementation.AllocateString(value.Length + 1);
        newPtr.Write(value);

        AllocatorImplementation.Free(ptrAddress);

        return newPtr;
    }

    [WasmExport("replace")]
    public static int Replace(int ptrAddress, int replacementAddress)
    {
        var str = Utils.StringFromPtr(ptrAddress);
        var strReplacement = Utils.StringFromPtr(replacementAddress);

        var replace = str.Replace(str, strReplacement);
        var newPtr = AllocatorImplementation.AllocateString(replace.Length + 1);
        newPtr.Write(replace);
        
        AllocatorImplementation.Free(ptrAddress);

        return newPtr;
    }

    [WasmExport("split")]
    public static int Split(int ptrAddress, int seperatorAddress)
    {
        var seperator = Utils.StringFromPtr(seperatorAddress);
        var str = Utils.StringFromPtr(ptrAddress);

        var split = str.Split(seperator, StringSplitOptions.RemoveEmptyEntries);

        var newPtr = (Pointer) AllocatorImplementation.Allocate(split.Select(_ => _.Length + 1).Sum());
        foreach (var s in split)
        {
            newPtr.Write(s);
            newPtr += s.Length + 1;
        }
        
        AllocatorImplementation.Free(ptrAddress);

        return newPtr;
    }

    [WasmExport("substr")]
    public static int Substr(int ptrAddress, int start, int count)
    {
        var str = Utils.StringFromPtr(ptrAddress);

        var newPtr = AllocatorImplementation.AllocateString(count + 2);
        newPtr.Write(str.Substring(start, count));

        AllocatorImplementation.Free(ptrAddress);
        
        return newPtr;
    }

    [WasmExport("compare")]
    public static int Compare(int lhsAddress, int rhsAddress)
    {
        var str = Utils.StringFromPtr(lhsAddress);
        var strRhs = Utils.StringFromPtr(rhsAddress);

        var result = string.Compare(str, strRhs, StringComparison.Ordinal);

        return result;
    }
}
