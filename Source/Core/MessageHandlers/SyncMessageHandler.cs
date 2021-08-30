using System.IO;
using Renci.SshNet;
using Slithin.Core.Remarkable;
using Slithin.Core.Services;
using Slithin.Core.Sync;
using Slithin.Core.Sync.Repositorys;
using Slithin.Messages;
using Slithin.Models;

namespace Slithin.Core.MessageHandlers
{
    public class SyncMessageHandler : IMessageHandler<SyncMessage>
    {
        private readonly DeviceRepository _device;
        private readonly LocalRepository _local;
        private readonly IPathManager _pathManager;
        private readonly ScpClient _scp;
        private readonly SynchronisationService _synchronisationService;

        public SyncMessageHandler(IPathManager pathManager,
                                  DeviceRepository device,
                                  ScpClient scp,
                                  LocalRepository local)
        {
            _pathManager = pathManager;
            _device = device;
            _scp = scp;
            _local = local;
            _synchronisationService = ServiceLocator.SyncService;
        }

        public void HandleMessage(SyncMessage message)
        {
            var notebooksDir = _pathManager.NotebooksDir;
            var customscreensDir = _pathManager.CustomScreensDir;

            if (message.Item.Direction == SyncDirection.ToDevice)
            {
                switch (message.Item.Type)
                {
                    case SyncType.Template:
                        if (message.Item.Action == SyncAction.Add)
                        {
                            _device.Add((Template)message.Item.Data);
                        }
                        else if (message.Item.Action == SyncAction.Remove)
                        {
                            _device.Remove((Template)message.Item.Data);
                        }
                        break;

                    case SyncType.TemplateConfig:
                        var templatesDir = _pathManager.TemplatesDir;

                        _scp.Upload(new FileInfo(Path.Combine(templatesDir, "templates.json")), PathList.Templates + "/templates.json");
                        break;

                    case SyncType.Notebook:
                        if (message.Item.Action == SyncAction.Remove)
                        {
                            _device.Remove((Metadata)message.Item.Data);
                        }
                        else if (message.Item.Action == SyncAction.Update)
                        {
                            if (message.Item.Data is Metadata md)
                            {
                                NotificationService.Show("Updating " + md.VisibleName);

                                _scp.Upload(new FileInfo(Path.Combine(notebooksDir, md.ID + ".metadata")), PathList.Documents + "/" + md.ID + ".metadata");
                            }
                        }
                        else if (message.Item.Action == SyncAction.Add)
                        {
                            if (message.Item.Data is Metadata md)
                            {
                                NotificationService.Show("Uploading " + md.VisibleName);

                                _scp.Upload(new FileInfo(Path.Combine(notebooksDir, md.ID + ".metadata")), PathList.Documents + "/" + md.ID + ".metadata");

                                if (md.Type == "DocumentType" && (md.Content.FileType == "pdf" || md.Content.FileType == "epub"))
                                {
                                    _scp.Upload(new FileInfo(Path.Combine(notebooksDir, md.ID + "." + md.Content.FileType)), PathList.Documents + "/" + md.ID + "." + md.Content.FileType);
                                    _scp.Upload(new FileInfo(Path.Combine(notebooksDir, md.ID + ".content")), PathList.Documents + "/" + md.ID + ".content");
                                }
                            }
                        }

                        break;

                    case SyncType.Screen:
                        if (message.Item.Data is CustomScreen cs && message.Item.Action == SyncAction.Add)
                        {
                            NotificationService.Show("Uploading Screen" + cs.Title);

                            _scp.Upload(new FileInfo(Path.Combine(customscreensDir, cs.Filename)), PathList.Screens + cs.Filename);
                        }
                        break;
                }
            }
            else
            {
                if (message.Item.Action == SyncAction.Remove)
                {
                    var data = (Metadata)message.Item.Data;
                    _local.Remove(data);
                    MetadataStorage.Local.Remove(data);
                    _synchronisationService.NotebooksFilter.Documents.Clear();

                    foreach (var mds in MetadataStorage.Local.GetByParent(_synchronisationService.NotebooksFilter.Folder))
                    {
                        _synchronisationService.NotebooksFilter.Documents.Add(mds);
                    }
                }
            }

            NotificationService.Hide();
        }
    }
}
