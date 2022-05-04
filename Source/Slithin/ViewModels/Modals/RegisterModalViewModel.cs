namespace Slithin.ViewModels.Modals;

public sealed class RegisterModalViewModel : ModalBaseViewModel
{
    private string _password;
    private string _username;

    public string Password
    {
        get { return _password; }
        set { SetValue(ref _password, value); }
    }

    public string Username
    {
        get { return _username; }
        set { SetValue(ref _username, value); }
    }
}
