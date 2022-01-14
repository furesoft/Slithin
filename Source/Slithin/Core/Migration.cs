using System.IO;
using Slithin.Core.Services;
using Slithin.Models;

namespace Slithin.Core;

public class Migration
{
    private LoginInfo _currentDevice;
    private ILoginService _loginService;
    private IMailboxService _mailboxService;
    private IPathManager _pathManager;

    public bool NeedsMigration { get; set; }

    public void StartMigration()
    {
        _pathManager = ServiceLocator.Container.Resolve<IPathManager>();
        _loginService = ServiceLocator.Container.Resolve<ILoginService>();
        _mailboxService = ServiceLocator.Container.Resolve<IMailboxService>();

        var baseDir = _pathManager.ConfigBaseDir;
        _currentDevice = _loginService.GetCurrentCredential();

        NeedsMigration = !baseDir.EndsWith(_currentDevice.Name);

        _mailboxService.PostAction(() =>
        {
            NotificationService.Show("Migrating. This can take a while");

            MigrateToNewFolderStructure();

            NotificationService.Hide();
        });
    }

    private void MigrateToNewFolderStructure()
    {
        var di = new DirectoryInfo(_pathManager.ConfigBaseDir);
        var newDir = _pathManager.ConfigBaseDir;
        //ToDo: implement moving algorithm of directories
        _pathManager.ConfigBaseDir = di.Parent.FullName;

        Directory.Move(_pathManager.ConfigBaseDir, newDir);

        _pathManager.ConfigBaseDir = newDirectoryName;
    }
}
