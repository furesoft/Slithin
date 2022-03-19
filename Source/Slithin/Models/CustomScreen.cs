using System.IO;
using System.Reflection;
using System.Windows.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using LiteDB;
using Renci.SshNet;
using Slithin.Core;
using Slithin.Core.Remarkable;
using Slithin.Core.Services;

namespace Slithin.Models;

public class CustomScreen : NotifyObject
{
    private IImage _image;

    public CustomScreen()
    {
        TransferCommand = new DelegateCommand(Upload);
        ResetCommand = new DelegateCommand(Reset);
    }

    public string Filename { get; set; }

    [BsonIgnore]
    public IImage Image
    {
        get => _image;
        set => SetValue(ref _image, value);
    }

    [BsonIgnore]
    public ICommand ResetCommand { get; set; }

    public string Title { get; set; }

    [BsonIgnore]
    public ICommand TransferCommand { get; set; }

    public void Load()
    {
        var pathManager = ServiceLocator.Container.Resolve<IPathManager>();

        if (!Directory.Exists(pathManager.CustomScreensDir))
        {
            return;
        }

        var path = Path.Combine(pathManager.CustomScreensDir, Filename);

        if (!File.Exists(path))
        {
            return;
        }

        using var strm = File.OpenRead(path);
        Image = Bitmap.DecodeToWidth(strm, 150);
    }

    private void Reset(object obj)
    {
        var pathManager = ServiceLocator.Container.Resolve<IPathManager>();

        var resourceName = $"Slithin.Resources.DefaultScreens.{Filename}";
        var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);

        var fileStream = File.OpenWrite(Path.Combine(pathManager.CustomScreensDir, Filename));
        stream.CopyTo(fileStream);

        fileStream.Dispose();
        stream.Dispose();

        Load();
        Upload(null);
    }

    private void Upload(object obj)
    {
        var mailboxService = ServiceLocator.Container.Resolve<IMailboxService>();
        var pathManager = ServiceLocator.Container.Resolve<IPathManager>();
        var scp = ServiceLocator.Container.Resolve<ScpClient>();
        var localisation = ServiceLocator.Container.Resolve<ILocalisationService>();
        var xochitl = ServiceLocator.Container.Resolve<Xochitl>();

        mailboxService.PostAction(() =>
        {
            NotificationService.Show(localisation.GetStringFormat("Uploading Screen '{0}'", Title));

            scp.Upload(new FileInfo(Path.Combine(pathManager.CustomScreensDir, Filename)), PathList.Screens + Filename);

            xochitl.ReloadDevice();

            NotificationService.Hide();
        });
    }
}
