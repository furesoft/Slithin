using System.IO;
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
        TransferCommand = new DelegateCommand(Transfer);
    }

    public string Filename { get; set; }

    [BsonIgnore]
    public IImage Image
    {
        get => _image;
        set => SetValue(ref _image, value);
    }

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

    private void Transfer(object obj)
    {
        var mailboxService = ServiceLocator.Container.Resolve<IMailboxService>();
        var pathManager = ServiceLocator.Container.Resolve<IPathManager>();
        var scp = ServiceLocator.Container.Resolve<ScpClient>();

        mailboxService.PostAction(() =>
        {
            //upload screen
            NotificationService.Show($"Uploading Screen '{Title}'");

            scp.Upload(new FileInfo(Path.Combine(pathManager.CustomScreensDir, Filename)), PathList.Screens + Filename);

            TemplateStorage.Instance.Apply();
            NotificationService.Hide();
        });
    }
}
