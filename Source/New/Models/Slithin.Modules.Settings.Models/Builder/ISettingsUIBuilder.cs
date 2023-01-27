using System.ComponentModel;
using Avalonia.Controls;

namespace Slithin.Modules.Settings.Models.Builder;

public interface ISettingsUiBuilder
{
    void RegisterControlProvider<TAttr, TProvider>();
    Control Build(IEnumerable<INotifyPropertyChanged> settingModels);
}
