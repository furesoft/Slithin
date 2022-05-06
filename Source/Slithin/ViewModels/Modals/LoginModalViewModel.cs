using System.Windows.Input;
using Slithin.Controls.Navigation;
using Slithin.Core.MVVM;
using Slithin.UI;

namespace Slithin.ViewModels.Modals;

public sealed class LoginModalViewModel : ModalBaseViewModel
{
    private string _password;
    private string _username;

    public LoginModalViewModel()
    {
        OnLoad();

        ShowRegisterCommand = new DelegateCommand(_ =>
        {
            var frame = Frame.GetFrame("loginFrame");

            if (frame.CanGoBack)
            {
                frame.GoBack();
            }
            else if (frame.CanGoForward)
            {
                frame.GoForward();
            }
        });
    }

    public string Password
    {
        get { return _password; }
        set { SetValue(ref _password, value); }
    }

    public ICommand ShowRegisterCommand { get; set; }

    public string Username
    {
        get { return _username; }
        set { SetValue(ref _username, value); }
    }

    public override void OnLoad()
    {
        base.OnLoad();

        Frame.GetFrame("loginFrame").Navigate(typeof(RegisterFramePage));
        Frame.GetFrame("loginFrame").Navigate(typeof(LoginFramePage));
    }
}
