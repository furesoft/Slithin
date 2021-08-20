using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slithin.Core.Services;
using Slithin.Core.Sync.Repositorys;
using Slithin.Messages;
using Slithin.Tools;

namespace Slithin.Core.MessageHandlers
{
    public class InitStorageMessageHandler : IMessageHandler<InitStorageMessage>
    {
        private readonly DeviceRepository _device;
        private readonly ILoadingService _loadingService;

        public InitStorageMessageHandler(DeviceRepository device, ILoadingService loadingService)
        {
            _device = device;
            _loadingService = loadingService;
        }

        public void HandleMessage(InitStorageMessage message)
        {
            NotificationService.Show("Downloading Screens");
            _device.DownloadCustomScreens();

            _device.GetTemplates();

            _loadingService.LoadTemplates();

            ServiceLocator.Container.Resolve<BackupTool>().Invoke(null);
        }
    }
}
