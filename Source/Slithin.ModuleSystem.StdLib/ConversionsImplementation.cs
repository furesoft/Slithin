namespace Slithin.ModuleSystem.StdLib;

[WasmExport("conversions")]
public class ConversionsImplementation
{
    [WasmExport("intToString")]
    public static void IntToString(int value, int dstAddress)
    {
        var dst = new Pointer(dstAddress);
        dst.Write(value.ToString());
    }

    [WasmExport("stringToInt")]
    public static int StringToInt(int ptrAddress)
    {
        var dst = new Pointer(ptrAddress);
        var str = dst.ReadString(StringImplementation.Length(ptrAddress));

        return int.Parse(str);
    }
}