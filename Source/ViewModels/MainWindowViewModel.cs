using Slithin.Core;
using Slithin.Core.Scripting;
using Slithin.Core.Services;
using Slithin.Core.Sync.Repositorys;
using Slithin.Messages;

namespace Slithin.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public MainWindowViewModel(EventStorage events, IMailboxService mailboxService, IVersionService versionService, LocalRepository localRepository)
        {
            if (versionService.GetLocalVersion() < versionService.GetDeviceVersion())
            {
                events.Invoke("newVersionAvailable");
                localRepository.UpdateVersion(versionService.GetDeviceVersion());
            }

            mailboxService.Post(new CheckForUpdateMessage());
        }
    }
}
