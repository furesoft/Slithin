using System.Windows.Input;
using Slithin.Controls;
using Slithin.Core;

namespace Slithin.ViewModels.Modals;

public class ModalBaseViewModel : BaseViewModel
{
    public ICommand AcceptCommand { get; set; }
    public ICommand CancelCommand { get; set; }

    public override void OnLoad()
    {
        base.OnLoad();

        CancelCommand = new DelegateCommand(_ => DialogService.Close());
    }
}
