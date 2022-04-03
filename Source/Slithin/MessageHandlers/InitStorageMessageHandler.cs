using Slithin.Core;
using Slithin.Core.Messaging;
using Slithin.Core.Services;
using Slithin.Core.Sync.Repositorys;
using Slithin.Messages;
using Slithin.Tools;

namespace Slithin.MessageHandlers;

public class InitStorageMessageHandler : IMessageHandler<InitStorageMessage>
{
    private readonly DeviceRepository _device;
    private readonly ILoadingService _loadingService;
    private readonly ILocalisationService _localisationService;

    public InitStorageMessageHandler(
        DeviceRepository device,
        ILocalisationService localisationService,
        ILoadingService loadingService)
    {
        _device = device;
        _localisationService = localisationService;
        _loadingService = loadingService;
    }

    public void HandleMessage(InitStorageMessage message)
    {
        NotificationService.Show(_localisationService.GetString("Downloading Screens"));
        _device.DownloadCustomScreens();

        _device.GetTemplates();

        _loadingService.LoadTemplates();

        ServiceLocator.Container.Resolve<BackupTool>().Invoke(null);
    }
}
