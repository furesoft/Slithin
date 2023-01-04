using Slithin.Core.MVVM;
using Slithin.Entities;

namespace Slithin.Modules.FirstStart.ViewModels;

internal class LoginInfoViewModel : BaseViewModel
{
    public LoginInfo SelectedLogin { get; set; } = new();
}
