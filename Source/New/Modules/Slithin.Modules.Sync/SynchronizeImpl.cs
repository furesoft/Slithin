using AuroraModularis.Core;
using Slithin.Modules.Device.Models;
using Slithin.Modules.Sync.Models;
using Slithin.Modules.UI.Models;

namespace Slithin.Modules.Sync;

public class SynchronizeImpl : ISynchronizeService
{
    public async void Synchronize()
    {
        await Task.Run(() =>
        {
            ServiceContainer.Current.Resolve<INotificationService>().ShowStatus("Synchronizing Device");
            var notebooks = ServiceContainer.Current.Resolve<IRemarkableDevice>().FetchFilesWithModified("/home/root/.local/share/remarkable/xochitl");
            var a = 1;
        });
    }
}
