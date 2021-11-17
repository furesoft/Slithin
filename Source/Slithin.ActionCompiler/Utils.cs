using System.Runtime.InteropServices;

namespace Slithin.ActionCompiler;

public static class Utils
{
    public static T FromBytes<T>(byte[] arr)
        where T : struct
    {
        var str = new T();

        var size = Marshal.SizeOf(str);
        var ptr = Marshal.AllocHGlobal(size);

        Marshal.Copy(arr, 0, ptr, size);

        str = (T) Marshal.PtrToStructure(ptr, str.GetType());
        Marshal.FreeHGlobal(ptr);

        return str;
    }

    public static byte[] GetBytes<T>(T str)
        where T : struct
    {
        var size = Marshal.SizeOf(str);
        var arr = new byte[size];

        var ptr = Marshal.AllocHGlobal(size);
        Marshal.StructureToPtr(str, ptr, true);
        Marshal.Copy(ptr, arr, 0, size);
        Marshal.FreeHGlobal(ptr);
        return arr;
    }
}

//ScriptInfo als data hinzufügen
//falls ui-xaml vorhanden, laden und als serialized in custom section speichern
//compilation des scripts mit start funktion in module