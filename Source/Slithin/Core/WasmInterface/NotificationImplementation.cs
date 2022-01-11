using Slithin.Controls;
using Slithin.ModuleSystem;
using Slithin.ModuleSystem.StdLib;

namespace Slithin.Core.WasmInterface;

[WasmExport("notification")]
public class NotificationImplementation
{
    [WasmExport("hide")]
    public static void Hide()
    {
        NotificationService.Hide();
    }

    [WasmExport("prompt")]
    public static int Prompt(int headerAddress)
    {
        var header = Utils.StringFromPtr(headerAddress);

        var result = DialogService.ShowPrompt(header, null).Result;

        var resultPtr = (Pointer)0;//ToDo: need to be allocated

        if (result != null)
        {
            resultPtr.Write(result);
        }
        else
        {
            resultPtr.Write(0);
        }

        return resultPtr;
    }

    [WasmExport("show")]
    public static void Show(int messageAddress)
    {
        NotificationService.Show(Utils.StringFromPtr(messageAddress));
    }
}
