namespace Slithin.ModuleSystem.StdLib;

[WasmExport("conversions")]
public class ConversionsImplementation
{
    [WasmExport("intToString")]
    public static void IntToString(int value, Pointer dst)
    {
        dst.Write(value.ToString());
    }
}