using System;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Input;
using Renci.SshNet;
using Slithin.Core;
using Slithin.Core.Remarkable;
using Slithin.Core.Services;
using Slithin.Core.Sync.Repositorys;
using Slithin.Messages;

namespace Slithin.Commands;

public class SynchronizeCommand : ICommand
{
    private readonly SshClient _client;
    private readonly ILocalisationService _localisationService;
    private readonly LocalRepository _localRepository;
    private readonly IMailboxService _mailboxService;
    private readonly IPathManager _pathManager;

    public SynchronizeCommand(IMailboxService mailboxService,
        LocalRepository localRepository,
        IPathManager pathManager,
        ILocalisationService localisationService,
        SshClient client)
    {
        _mailboxService = mailboxService;
        _localRepository = localRepository;
        _pathManager = pathManager;
        _localisationService = localisationService;
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
            NotificationService.Show(_localisationService.GetString(
                "Your remarkable is not reachable. Please check your connection and restart Slithin"));

            return;
        }

        if (!_localRepository.GetTemplates().Any() && !Directory.GetFiles(_pathManager.TemplatesDir).Any())
        {
            _mailboxService.Post(new InitStorageMessage());
        }

        _mailboxService.Post(new CollectSyncNotebooksMessage());

        SyncDeviceDeletions();

        //ModuleEventStorage.Invoke("OnSynchonized", 0);
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
            //ToDo: Sync Device Deletions
            //PostDeviceDeletion(file);
        }
    }
}
