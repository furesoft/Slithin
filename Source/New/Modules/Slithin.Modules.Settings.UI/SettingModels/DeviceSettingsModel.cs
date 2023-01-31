using AuroraModularis.Logging.Models;
using Slithin.Core;
using Slithin.Entities;
using Slithin.Modules.BaseServices.Models;
using Slithin.Modules.Repository.Models;
using Slithin.Modules.Settings.Models;
using Slithin.Modules.Settings.Models.Builder.Attributes;

namespace Slithin.Modules.Settings.UI.SettingModels;

[DisplaySettings("Device")]
public class DeviceSettingsModel : SavableSettingsModel
{
    private readonly ILoginService _loginService;
    private readonly IPathManager _pathManager;
    private readonly ILogger _logger;
    private readonly LoginInfo _credential;

    public DeviceSettingsModel(ILoginService loginService, IPathManager pathManager, ILogger logger)
    {
        _loginService = loginService;
        _pathManager = pathManager;
        _logger = logger;

        _credential = loginService.GetCurrentCredential();
    }
    
    [Settings("Device Name")]
    public string DeviceName
    {
        get => _credential.Name;
        set =>
            new Action<string>((v) =>
            {
                UpdateDeviceName(v);
                OnChange();
            }).Debounce()(value);
    }
    
    private void UpdateDeviceName(string newName)
    {
        var oldName = DeviceName;

        if (string.IsNullOrEmpty(newName))
        {
            return;
        }

        _pathManager.ReLink(newName);

        var path = new DirectoryInfo(_pathManager.ConfigBaseDir);

        if (path.Exists)
        {
            return;
        }

        _pathManager.ReLink(oldName);
        path = new(_pathManager.ConfigBaseDir);

        _credential.Name = newName;

        _pathManager.ReLink(_credential.Name);

        path.MoveTo(_pathManager.ConfigBaseDir);
    }
    
    public override void Save()
    {
        _loginService.UpdateLoginCredential(_credential);

        _logger.Info("Setting changed 'Device Name'");
    }
}
