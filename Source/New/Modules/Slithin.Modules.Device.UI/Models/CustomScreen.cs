using System.Reflection;
using System.Windows.Input;
using AuroraModularis.Core;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using LiteDB;
using Slithin.Core.MVVM;
using Slithin.Modules.BaseServices.Models;
using Slithin.Modules.Device.Models;

namespace Slithin.Modules.Device.UI.Models;

internal class CustomScreen : NotifyObject
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
        var pathManager = Container.Current.Resolve<IPathManager>();

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
        var pathManager = Container.Current.Resolve<IPathManager>();

        var resourceName = $"Slithin.Modules.Device.UI.Resources.DefaultScreens.{Filename}";
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
        var device = Container.Current.Resolve<IRemarkableDevice>();
        var pathManager = Container.Current.Resolve<IPathManager>();
        var pathList = Container.Current.Resolve<PathList>();

        device.Upload(new FileInfo(Path.Combine(pathManager.CustomScreensDir, Filename)), pathList.Screens + Filename);
        /*
        var mailboxService = ServiceLocator.Container.Resolve<IMailboxService>();
        var pathManager = ServiceLocator.Container.Resolve<IPathManager>();
        var scp = ServiceLocator.Container.Resolve<ISSHService>();
        var localisation = ServiceLocator.Container.Resolve<ILocalisationService>();
        var xochitl = ServiceLocator.Container.Resolve<Xochitl>();

        mailboxService.PostAction(() =>
        {
            NotificationService.Show(localisation.GetStringFormat("Uploading Screen '{0}'", Title));

            scp.Upload(new FileInfo(Path.Combine(pathManager.CustomScreensDir, Filename)), PathList.Screens + Filename);

            xochitl.ReloadDevice();
        });
        */
    }
}
