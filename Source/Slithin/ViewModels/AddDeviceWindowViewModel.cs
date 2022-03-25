using System.IO;
using System.Linq;
using System.Windows.Input;
using Material.Styles;
using Slithin.Core;
using Slithin.Core.Services;
using Slithin.Core.Validators;
using Slithin.Models;

namespace Slithin.ViewModels;

public class AddDeviceWindowViewModel : BaseViewModel
{
    private readonly ILoginService _loginService;
    private readonly IPathManager _pathManager;
    private readonly LoginInfoValidator _validator;
    private LoginInfo _selectedLogin;

    private string _sshKeyFilename;

    public AddDeviceWindowViewModel(LoginInfoValidator validator, ILoginService loginService, IPathManager pathManager)
    {
        CancelCommand = new DelegateCommand(Cancel);
        AddCommand = new DelegateCommand(Add);
        _validator = validator;
        _loginService = loginService;
        _pathManager = pathManager;
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

    public string SshKeyFilename
    {
        get { return _sshKeyFilename; }
        set { SetValue(ref _sshKeyFilename, value); }
    }

    private void Add(object obj)
    {
        if (!string.IsNullOrEmpty(_sshKeyFilename))
        {
            SelectedLogin.Key = File.ReadAllBytes(_sshKeyFilename);
        }
        else
        {
            var result = _validator.Validate(SelectedLogin);

            if (!result.IsValid)
            {
                SnackbarHost.Post(result.Errors.First().ToString(), "addDevice");
                return;
            }
        }

        ParentViewModel.LoginCredentials.Add(SelectedLogin);
        ParentViewModel.SelectedLogin = SelectedLogin;

        _loginService.RememberLoginCredencials(SelectedLogin);
        _pathManager.InitDeviceDirectory();

        this.RequestClose();
    }

    private void Cancel(object obj)
    {
        this.RequestClose();
    }
}
