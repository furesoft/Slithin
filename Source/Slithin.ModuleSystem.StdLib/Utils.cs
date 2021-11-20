namespace Slithin.ModuleSystem.StdLib;

public static class Utils
{
    public static string StringFromPtr(int ptrAddress)
    {
        var ptr = new Pointer(ptrAddress);
        var len = StringImplementation.Length(ptrAddress);

        var str = new string(ptr.ReadString(len));

        return str;
    }
}