using System;
using System.Linq;
using Slithin.Core;
using Slithin.Core.Remarkable;
using Slithin.Core.Remarkable.Cloud;

namespace Slithin.ViewModels
{
    public class DevicePageViewModel : BaseViewModel
    {
        public DevicePageViewModel()
        {
            SyncService.CustomScreens.Add(new CustomScreen { Title = "Starting", Filename = "starting.png" });
            SyncService.CustomScreens.Add(new CustomScreen { Title = "Power Off", Filename = "poweroff.png" });
            SyncService.CustomScreens.Add(new CustomScreen { Title = "Suspended", Filename = "suspended.png" });
            SyncService.CustomScreens.Add(new CustomScreen { Title = "Rebooting", Filename = "rebooting.png" });

            foreach (var cs in ServiceLocator.SyncService.CustomScreens)
            {
                cs.Load();
            }

            Version = ServiceLocator.Device.GetVersion().ToString();

            var ru = Storage.RequestUpload();
            Storage.Upload(ru, @"C:\Users\chris\Downloads\Ernährungstagebuch.pdf");

            var md = new Metadata();
            md.ID = ru.ID;
            md.VisibleName = "Uploaded from Slithin";
            md.Parent = "";
            md.Version = 1;
            md.Type = "DocumentType";

            Storage.UpdateMetadata(md);

            var folde = new Metadata();
            folde.ID = Guid.NewGuid().ToString();
            folde.VisibleName = "Slitin Folder";
            folde.Parent = "";
            folde.Version = 1;
            folde.Type = "CollectionType";

            Storage.UpdateMetadata(folde);
        }

        public string Version { get; set; }
    }
}
