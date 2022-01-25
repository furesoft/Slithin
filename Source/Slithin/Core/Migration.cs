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

    public Migration()
    {
        var pathManager = ServiceLocator.Container.Resolve<IPathManager>();

        NeedsMigration = File.Exists(Path.Combine(pathManager.ConfigBaseDir, ".version"));
    }

    public bool NeedsMigration { get; set; }

    public void StartMigration()
    {
        _pathManager = ServiceLocator.Container.Resolve<IPathManager>();
        _loginService = ServiceLocator.Container.Resolve<ILoginService>();
        _mailboxService = ServiceLocator.Container.Resolve<IMailboxService>();

        var baseDir = _pathManager.ConfigBaseDir;
        _currentDevice = _loginService.GetCurrentCredential();

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

        _pathManager.ConfigBaseDir = di.Parent.Parent.FullName;

        MoveFile(_pathManager.ConfigBaseDir, newDir, "templates.json");
        MoveFile(_pathManager.ConfigBaseDir, newDir, ".version");

        MoveDirectory(_pathManager.BackupsDir, newDir);
        MoveDirectory(_pathManager.CustomScreensDir, newDir);
        MoveDirectory(_pathManager.NotebooksDir, newDir);
        MoveDirectory(_pathManager.ScriptsDir, newDir);
        MoveDirectory(_pathManager.TemplatesDir, newDir);

        _pathManager.ConfigBaseDir = newDir;
    }

    private void MoveDirectory(string dir, string newDir)
    {
        var folder = new DirectoryInfo(dir).Name;

        if (Directory.Exists(Path.Combine(newDir, folder)))
        {
            Directory.Delete(Path.Combine(newDir, folder));
        }

        Directory.Move(dir, Path.Combine(newDir, folder));
    }

    private void MoveFile(string dir, string newDir, string file)
    {
        var folder = new DirectoryInfo(dir).Name;

        if (File.Exists(Path.Combine(newDir, file)))
        {
            File.Delete(Path.Combine(newDir, file));
        }

        File.Move(Path.Combine(dir, file), Path.Combine(newDir, file));
    }
}
