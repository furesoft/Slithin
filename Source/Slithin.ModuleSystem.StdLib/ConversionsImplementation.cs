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
}