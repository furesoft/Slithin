using System.ComponentModel;
using Avalonia.Controls;

namespace Slithin.Modules.Settings.Models.Builder;

public interface ISettingsUiBuilder
{
    void RegisterControlProvider<TAttr, TProvider>();

    void RegisterSettingsModel(INotifyPropertyChanged model);
    Control Build();
}
