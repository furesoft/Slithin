using Avalonia.Controls;

namespace Slithin.Modules.Settings.Models.Builder;

public interface ISettingsUiBuilder
{
    void RegisterControlProvider<TProvider>();

    void RegisterSettingsModel<T>();
    Control Build();
}
