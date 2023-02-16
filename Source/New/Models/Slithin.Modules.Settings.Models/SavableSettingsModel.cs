using Slithin.Core.MVVM;

namespace Slithin.Modules.Settings.Models;

public abstract class SavableSettingsModel : NotifyObject
{
    public abstract void Save();
}
