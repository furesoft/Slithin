using Slithin.Core.Services;
using Slithin.ModuleSystem;
using Slithin.ModuleSystem.StdLib;

namespace Slithin.Core.WasmInterface;

[WasmExport("setting")]
public class SettingsImplementation
{
    [WasmExport("load")]
    public static void Load(int keyAddress, int resultAddress)
    {
        var settingsService = ServiceLocator.Container.Resolve<ISettingsService>();
        var settings = settingsService.GetSettings();

        var value = settings.Get(Utils.StringFromPtr(keyAddress));

        var resultPtr = (Pointer)resultAddress;

        if (value != null)
        {
            resultPtr.Write(value);
        }
        else
        {
            resultPtr.Write(0);
        }
    }

    [WasmExport("store")]
    public static void Store(int keyAddress, int valueAddress)
    {
        var settingsService = ServiceLocator.Container.Resolve<ISettingsService>();
        var settings = settingsService.GetSettings();

        settings.Put(Utils.StringFromPtr(keyAddress), Utils.StringFromPtr(valueAddress));
    }
}
