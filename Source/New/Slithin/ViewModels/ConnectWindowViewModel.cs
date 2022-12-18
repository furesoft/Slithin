using System.Collections.ObjectModel;
using System.Windows.Input;
using AuroraModularis.Core;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Slithin.Core;
using Slithin.Core.MVVM;
using Slithin.Entities;
using Slithin.Modules.Device.Models;
using Slithin.Modules.Repository.Models;
using Slithin.Views;

namespace Slithin.ViewModels;

public class ConnectionWindowViewModel : BaseViewModel
{
    private readonly ILoginService _loginService;
    private readonly IRemarkableDevice _remarkableDevice;
    private ObservableCollection<LoginInfo> _loginCredentials;

    private LoginInfo _selectedLogin;

    public ConnectionWindowViewModel(ILoginService loginService, IRemarkableDevice remarkableDevice)
    {
        ConnectCommand = new DelegateCommand(Connect);
        HelpCommand = new DelegateCommand(Help);
        OpenAddDeviceCommand = new DelegateCommand(OpenAddDevice);
        RemoveDeviceCommand = new DelegateCommand(RemoveDevice);

        SelectedLogin = new LoginInfo();
        _loginService = loginService;
        _remarkableDevice = remarkableDevice;
    }

    public ICommand ConnectCommand { get; set; }

    public ICommand HelpCommand { get; set; }

    public ObservableCollection<LoginInfo> LoginCredentials
    {
        get => _loginCredentials;
        set => SetValue(ref _loginCredentials, value);
    }

    public ICommand OpenAddDeviceCommand { get; set; }

    public ICommand RemoveDeviceCommand { get; set; }

    public LoginInfo SelectedLogin
    {
        get => _selectedLogin;
        set => SetValue(ref _selectedLogin, value);
    }

    public override void OnLoad()
    {
        base.OnLoad();
        InitDeviceList();
    }

    private void InitDeviceList()
    {
        var li = _loginService.GetLoginCredentials();

        for (var i = 0; i < li.Length; i++)
        {
            if (string.IsNullOrEmpty(li[i].Name))
            {
                li[i].Name = "Device " + (i + 1);
            }
        }

        SelectedLogin = li.FirstOrDefault() ?? new LoginInfo();

        LoginCredentials = new(li);
    }

    private void RemoveDevice(object obj)
    {
        _loginService.Remove(SelectedLogin);
        SelectedLogin = new();

        InitDeviceList();
    }

    private void Connect(object obj)
    {
        var ip = IPAddress.Parse(SelectedLogin.IP);

        if (string.IsNullOrEmpty(SelectedLogin.Name))
        {
            SelectedLogin.Name = "DefaultDevice";
        }

        _loginService.SetLoginCredential(SelectedLogin);
        _remarkableDevice.Connect(ip, SelectedLogin.Password);

        var mainWindow = new MainWindow();
        ApplyViewModel<MainWindowViewModel>(mainWindow);
        mainWindow.Show();

        RequestClose();
    }

    private void Help(object obj)
    {
        Utils.OpenUrl("https://tinyurl.com/remarkable-ssh");
    }

    private void OpenAddDevice(object obj)
    {
        var wndw = new AddDeviceWindow();
        var vm = Container.Current.Resolve<AddDeviceWindowViewModel>();
        vm.ParentViewModel = this;

        wndw.DataContext = vm;

        vm.OnRequestClose += () => wndw.Close();

        wndw.ShowDialog(((IClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime).MainWindow);
    }
}
