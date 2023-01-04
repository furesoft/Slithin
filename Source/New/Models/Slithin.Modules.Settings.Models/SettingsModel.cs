using Avalonia;
using LiteDB;
using Slithin.Core.MVVM;
using Slithin.Modules.Resources.Models;

namespace Slithin.Modules.Settings.Models;

public class SettingsModel : NotifyObject
{
    private bool _isBigMenuMode = true;
    public ObjectId? _id { get; set; }

    public MarketplaceUser MarketplaceCredential { get; set; }

    public bool AutomaticScreenRecovery { get; set; }
    public bool AutomaticTemplateRecovery { get; set; }
    public Dictionary<string, object> CustomSettings { get; set; } = new();

    public bool IsBigMenuMode
    {
        get { return _isBigMenuMode; }
        set { SetValue(ref _isBigMenuMode, value); }
    }

    public bool IsDarkMode { get; set; }
    public bool IsFirstStart { get; set; } = true;
    public bool UsedMultiScreen { get; set; }
    public Rect WindowPosition { get; set; }

    public T Get<T>(string key)
    {
        if (CustomSettings.ContainsKey(key))
        {
            return (T)CustomSettings[key];
        }

        return default;
    }

    public void Put(string key, object value)
    {
        if (CustomSettings.ContainsKey(key))
        {
            CustomSettings[key] = value;
        }
        else
        {
            CustomSettings.Add(key, value);
        }
    }
}
