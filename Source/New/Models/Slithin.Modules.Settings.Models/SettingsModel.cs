using Avalonia;
using LiteDB;
using Slithin.Core.MVVM;

namespace Slithin.Core;

public class SettingsModel : NotifyObject
{
    public ObjectId _id { get; set; }

    public bool AutomaticScreenRecovery { get; set; }
    public bool AutomaticTemplateRecovery { get; set; }
    public bool AutomaticUpdates { get; set; } = true;
    public Dictionary<string, string> CustomSettings { get; set; } = new();
    public bool IsBigMenuMode { get; set; } = true;
    public bool IsDarkMode { get; set; }
    public bool IsFirstStart { get; set; } = true;
    public bool UsedMultiScreen { get; set; }
    public Rect WindowPosition { get; set; }

    public string Get(string key)
    {
        if (CustomSettings.ContainsKey(key))
        {
            return CustomSettings[key];
        }

        return string.Empty;
    }

    public void Put(string key, string value)
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
