using System;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Input;
using Newtonsoft.Json;
using Renci.SshNet;
using Slithin.Core.Remarkable;
using Slithin.Core.Services;
using Slithin.Core.Sync;
using Slithin.Core.Sync.Repositorys;
using Slithin.Messages;

namespace Slithin.Core.Commands;

public class SynchronizeCommand : ICommand
{
    private readonly SshClient _client;
    private readonly LocalRepository _localRepository;
    private readonly IMailboxService _mailboxService;
    private readonly ModuleEventStorage _moduleEventStorage;
    private readonly IPathManager _pathManager;

    public SynchronizeCommand(ModuleEventStorage moduleEventStorage,
        IMailboxService mailboxService,
        LocalRepository localRepository,
        IPathManager pathManager,
        SshClient client)
    {
        _moduleEventStorage = moduleEventStorage;
        _mailboxService = mailboxService;
        _localRepository = localRepository;
        _pathManager = pathManager;
        _client = client;
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        var pingSender = new Ping();

        var buffer = Encoding.ASCII.GetBytes(new string('a', 32));

        const int Timeout = 10000;

        var options = new PingOptions(64, true);

        var reply = pingSender.Send(ServiceLocator.Container.Resolve<ScpClient>().ConnectionInfo.Host, Timeout, buffer,
            options);

        if (reply.Status != IPStatus.Success)
        {
            NotificationService.Show(
                "Your remarkable is not reachable. Please check your connection and restart Slithin");

            return;
        }

        //_eventStorage.Invoke("beforeSync", new[] { ServiceLocator.SyncService.PersistentSyncQueue.FindAll() });

        if (!_localRepository.GetTemplates().Any() && !Directory.GetFiles(_pathManager.TemplatesDir).Any())
        {
            _mailboxService.Post(new InitStorageMessage());
        }

        NotificationService.Show("Syncing ...");

        _mailboxService.Post(new CollectSyncNotebooksMessage());

        SyncDeviceDeletions();

        foreach (var item in ServiceLocator.SyncService.PersistentSyncQueue.FindAll())
        {
            _mailboxService.Post(new SyncMessage {Item = item}); // redirect sync job to mailbox for asynchronity
        }

        ServiceLocator.SyncService.SyncQueue.AnalyseAndAppend();

        ServiceLocator.SyncService.PersistentSyncQueue.DeleteAll();
        ServiceLocator.SyncService.SyncQueue.Clear();

        _moduleEventStorage.Invoke("afterSync");
    }

    public void RaiseExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    private void SyncDeviceDeletions()
    {
        var sshCommand = _client.RunCommand("ls -p " + PathList.Documents);
        var deviceFiles = sshCommand.Result
            .Split('\n', StringSplitOptions.RemoveEmptyEntries).Where(_ => _.EndsWith(".metadata"));
        var localFiles = Directory.GetFiles(_pathManager.NotebooksDir).Where(_ => _.EndsWith(".metadata"))
            .Select(Path.GetFileName);

        var deltaLocalFiles = localFiles.Except(deviceFiles);

        foreach (var file in deltaLocalFiles)
        {
            PostDeviceDeletion(file);
        }
    }

    private void PostDeviceDeletion(string file)
    {
        var item = new SyncItem
        {
            Data = JsonConvert.DeserializeObject<Metadata>(
                File.ReadAllText(Path.Combine(_pathManager.NotebooksDir, file))),
            Direction = SyncDirection.ToLocal,
            Action = SyncAction.Remove
        };

        ((Metadata)item.Data)!.ID = Path.GetFileNameWithoutExtension(file);

        _mailboxService.Post(new SyncMessage {Item = item});
    }
}
