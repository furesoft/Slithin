using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Controls;

namespace Slithin.Modules.Settings.Models.Builder;

public interface ISettingsUiBuilder
{
    void RegisterControlProvider<TAttr, TProvider>();
    Control BuildSection(INotifyPropertyChanged settingsObject, [CallerArgumentExpression(nameof(settingsObject))] string settingsExpr = null);
}
