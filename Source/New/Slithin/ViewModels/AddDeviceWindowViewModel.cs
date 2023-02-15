using System.Windows.Input;
using Slithin.Core.MVVM;
using Slithin.Entities;
using Slithin.Modules.BaseServices.Models;
using Slithin.Modules.Repository.Models;

namespace Slithin.ViewModels;

public class AddDeviceWindowViewModel : BaseViewModel
{
    private readonly ILoginService _loginService;
    private readonly IPathManager _pathManager;
    private readonly IDeviceDiscovery _deviceDiscovery;
    private LoginInfo _selectedLogin;

    public AddDeviceWindowViewModel(ILoginService loginService, IPathManager pathManager, IDeviceDiscovery deviceDiscovery)
    {
        CancelCommand = new DelegateCommand(Cancel);
        AddCommand = new DelegateCommand(Add);
        _loginService = loginService;
        _pathManager = pathManager;
        _deviceDiscovery = deviceDiscovery;
        SelectedLogin = new();
    }

    public ICommand AddCommand { get; set; }

    public ICommand CancelCommand { get; set; }

    public ConnectionWindowViewModel ParentViewModel { get; set; }

    public LoginInfo SelectedLogin
    {
        get => _selectedLogin;
        set => SetValue(ref _selectedLogin, value);
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        var ip = _deviceDiscovery.Discover();

        if (ip != null)
        {
            SelectedLogin.IP = ip.ToString();
        }
    }

    private void Add(object obj)
    {
        ParentViewModel.LoginCredentials.Add(SelectedLogin);
        ParentViewModel.SelectedLogin = SelectedLogin;

        _loginService.RememberLoginCredencials(SelectedLogin);
        _loginService.SetLoginCredential(SelectedLogin);

        _pathManager.ReLink(SelectedLogin.Name);
        _pathManager.InitDeviceDirectory();

        this.RequestClose();
    }

    private void Cancel(object obj)
    {
        this.RequestClose();
    }
}
