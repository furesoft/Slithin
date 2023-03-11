using AuroraModularis.Core;
using Slithin.Core.MVVM;
using Slithin.Modules.Sync.Models;

namespace Slithin.Modules.FirstStart.ViewModels;

public class FinishViewModel : BaseViewModel
{
    protected override async void OnLoad()
    {
        var syncService = ServiceContainer.Current.Resolve<ISynchronizeService>();

        await syncService.Synchronize(true);
    }
}
