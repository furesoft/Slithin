using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using Avalonia.Media;
using LiteDB;
using Newtonsoft.Json;
using Slithin.Core.MVVM;

namespace Slithin.Entities.Remarkable;

public class Template : NotifyObject, IEqualityComparer<Template>
{
    private IImage _image;

    public Template()
    {
        // TransferCommand = new DelegateCommand(Transfer);
    }

    [JsonProperty("categories")] public string[] Categories { get; set; }

    [JsonProperty("filename")] public string Filename { get; set; }

    [JsonProperty("iconCode")] public string IconCode { get; set; }

    [JsonIgnore]
    [BsonIgnore]
    public IImage Image
    {
        get => _image;
        set
        {
            if (_image == value)
            {
                return;
            }

            SetValue(ref _image, value);
        }
    }

    [JsonProperty("landscape")] public bool Landscape { get; set; }

    [JsonProperty("name")] public string Name { get; set; }

    [BsonIgnore]
    [JsonIgnore]
    public ICommand TransferCommand { get; set; }

    public bool Equals(Template x, Template y)
    {
        return x.Name.Equals(y.Name) && x.Filename.Equals(y.Filename);
    }

    public int GetHashCode([DisallowNull] Template obj)
    {
        return HashCode.Combine(obj.Filename, obj.Landscape, obj.Name);
    }

    /*
    private void Transfer(object obj)
    {
        var mailboxService = ServiceLocator.Container.Resolve<IMailboxService>();
        var pathManager = ServiceLocator.Container.Resolve<IPathManager>();
        var scp = ServiceLocator.Container.Resolve<ISSHService>();
        var xochitl = ServiceLocator.Container.Resolve<Xochitl>();

        mailboxService.PostAction(() =>
        {
            NotificationService.Show($"Uploading Template '{Name}'");

            scp.Upload(new FileInfo(Path.Combine(pathManager.TemplatesDir, Filename + ".png")), PathList.Templates + Filename + ".png");

            var tmpStorage = new TemplateStorage();

            var ms = new MemoryStream();

            scp.Download(PathList.Templates + "templates.json", ms);

            ms.Seek(0, SeekOrigin.Begin);

            var remoteTemplatesContent = new StreamReader(ms).ReadToEnd();
            tmpStorage.Templates = JsonConvert.DeserializeObject<TemplateStorage>(remoteTemplatesContent).Templates;
            tmpStorage.AppendTemplate(this);
            tmpStorage.Save();

            scp.Upload(new FileInfo(Path.Combine(pathManager.ConfigBaseDir, "templates.json")), PathList.Templates + "templates.json");

            xochitl.ReloadDevice();
        });
    }*/
}
