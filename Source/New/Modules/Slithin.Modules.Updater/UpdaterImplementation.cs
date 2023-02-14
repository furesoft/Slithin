using AuroraModularis.Core;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.UI.Models;
using Slithin.Modules.Updater.Models;

namespace Slithin.Modules.Updater;

internal class UpdaterImplementation : IUpdaterService
{
    public async Task<bool> CheckForUpdate()
    {
        return (await UpdateRepository.GetUpdatablePackages()).Count > 0;
    }

    public async Task StartUpdate()
    {
        var localisationService = ServiceContainer.Current.Resolve<ILocalisationService>();
        var notificationService = ServiceContainer.Current.Resolve<INotificationService>();

        
    }
}
