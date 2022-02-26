using System.IO;
using Renci.SshNet;
using Slithin.Core.Remarkable;
using Slithin.Core.Services;
using Slithin.Core.Sync;
using Slithin.Core.Sync.Repositorys;
using Slithin.Messages;

namespace Slithin.Core.MessageHandlers;

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
                        _device.AddTemplate((Template)message.Item.Data);
                    }
                    else if (message.Item.Action == SyncAction.Remove)
                    {
                        _device.RemoveTemplate((Template)message.Item.Data);
                    }

                    break;

                case SyncType.TemplateConfig:
                    var templatesDir = _pathManager.TemplatesDir;

                    _scp.Upload(new FileInfo(Path.Combine(templatesDir, "templates.json")),
                        PathList.Templates + "/templates.json");
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

                            md.Upload();
                        }
                    }
                    else if (message.Item.Action == SyncAction.Add)
                    {
                        if (message.Item.Data is Metadata md)
                        {
                            NotificationService.Show("Uploading " + md.VisibleName);

                            md.Upload();
                        }
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
