namespace Slithin.Modules.Repository.Models;

public interface IPathManager
{
    public string BackupsDir { get; }
    public string ConfigBaseDir { get; set; }
    public string CustomScreensDir { get; }
    public string DevicesDir { get; }
    public string NotebooksDir { get; }
    public string ScriptsDir { get; }
    public string SlithinDir { get; }
    public string TemplatesDir { get; }

    void Init();

    void InitDeviceDirectory();

    void ReLink(string deviceName);
}
