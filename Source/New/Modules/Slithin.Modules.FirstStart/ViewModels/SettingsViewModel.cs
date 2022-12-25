using Slithin.Core.MVVM;
using Slithin.Modules.Settings.Models;

namespace Slithin.Modules.FirstStart.ViewModels;

internal class SettingsViewModel : BaseViewModel
{
    public SettingsModel Settings { get; set; } = new();
}
