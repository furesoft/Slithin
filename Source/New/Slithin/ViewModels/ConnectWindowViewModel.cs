using System.Collections.ObjectModel;
using System.Windows.Input;
using Slithin.Core;
using Slithin.Core.MVVM;
using Slithin.Entities;
using Slithin.Modules.Repository.Models;

namespace Slithin.ViewModels;

public class ConnectionWindowViewModel : BaseViewModel
{
    private readonly ILoginService _loginService;
    private ObservableCollection<LoginInfo> _loginCredentials;

    private LoginInfo _selectedLogin;

    public ConnectionWindowViewModel(ILoginService loginService)
    {
        ConnectCommand = new DelegateCommand(Connect);
        HelpCommand = new DelegateCommand(Help);
        //OpenAddDeviceCommand = new DelegateCommand(OpenAddDevice);

        SelectedLogin = new LoginInfo();
        _loginService = loginService;
    }

    public ICommand ConnectCommand { get; set; }
    public ICommand HelpCommand { get; set; }

    public ObservableCollection<LoginInfo> LoginCredentials
    {
        get => _loginCredentials;
        set => SetValue(ref _loginCredentials, value);
    }

    public ICommand OpenAddDeviceCommand { get; set; }

    public LoginInfo SelectedLogin
    {
        get => _selectedLogin;
        set => SetValue(ref _selectedLogin, value);
    }

    public override void OnLoad()
    {
        base.OnLoad();

        var li = _loginService.GetLoginCredentials();

        for (var i = 0; i < li.Length; i++)
        {
            if (string.IsNullOrEmpty(li[i].Name))
            {
                li[i].Name = "Device " + (i + 1);
            }
        }

        SelectedLogin = li.FirstOrDefault() ?? new LoginInfo();

        LoginCredentials = new();
    }

    private void Connect(object obj)
    {
        var ip = IPAddress.Parse(SelectedLogin.IP);

        if (string.IsNullOrEmpty(SelectedLogin.Name))
        {
            SelectedLogin.Name = "DefaultDevice";
        }
    }

    private void Help(object obj)
    {
        Utils.OpenUrl("https://tinyurl.com/remarkable-ssh");
    }

    /*
    private void OpenAddDevice(object obj)
    {
        var wndw = new AddDeviceWindow();
        var vm = ServiceLocator.Container.Resolve<AddDeviceWindowViewModel>();
        vm.ParentViewModel = this;

        wndw.DataContext = vm;

        vm.OnRequestClose += () => wndw.Close();

        wndw.ShowDialog(((IClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime).MainWindow);
    }*/
}
