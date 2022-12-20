using System.Windows.Input;

namespace Slithin.Core.MVVM;

public class ModalBaseViewModel : BaseViewModel
{
    public ICommand AcceptCommand { get; set; }
    public ICommand CancelCommand { get; set; }

    public override void OnLoad()
    {
        base.OnLoad();

        //CancelCommand = new DelegateCommand(_ => DialogService.Close());
    }
}
