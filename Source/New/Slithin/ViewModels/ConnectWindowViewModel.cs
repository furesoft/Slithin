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
using Slithin.Modules.Settings.Models;
using Slithin.Modules.UI.Models;
using Slithin.Modules.Updater.Models;
using Slithin.Validators;
using Slithin.Views;

namespace Slithin.ViewModels;

public class ConnectionWindowViewModel : BaseViewModel
{
    private readonly ILoginService _loginService;
    private readonly IRemarkableDevice _remarkableDevice;
    private readonly ISettingsService _settingsService;
    private readonly IUpdaterService _updaterService;
    private readonly INotificationService _notificationService;
    private readonly LoginInfoValidator _validator;
    private ObservableCollection<LoginInfo> _loginCredentials;

    private LoginInfo _selectedLogin;

    public ConnectionWindowViewModel(ILoginService loginService,
                                     IRemarkableDevice remarkableDevice,
                                     ISettingsService settingsService,
                                     IUpdaterService updaterService,
                                     INotificationService notificationService,
                                     LoginInfoValidator validator)
    {
        ConnectCommand = new DelegateCommand(Connect);
        HelpCommand = new DelegateCommand(Help);
        OpenAddDeviceCommand = new DelegateCommand(OpenAddDevice);
        RemoveDeviceCommand = new DelegateCommand(RemoveDevice);

        SelectedLogin = new LoginInfo();
        _loginService = loginService;
        _remarkableDevice = remarkableDevice;
        _settingsService = settingsService;
        _updaterService = updaterService;
        _notificationService = notificationService;
        _validator = validator;
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

    protected override async void OnLoad()
    {
        if (!_settingsService.GetSettings().IsFirstStart)
        {
            RequestClose();
        }

        InitDeviceList();

        if (await _updaterService.CheckForUpdate())
        {
            //RequestClose();
            await _updaterService.StartUpdate();
        }
    }

    private void InitDeviceList()
    {
        var li = _loginService.GetLoginCredentials();

        for (var i = 0; i < li.Length; i++)
        {
            if (string.IsNullOrEmpty(li[i].Name))
            {
                li[i].Name = $"Device {(i + 1)}";
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

        var validationResult = _validator.Validate(SelectedLogin);

        if (!validationResult.IsValid)
        {
            _notificationService.ShowErrorNewWindow(validationResult.Errors.AsString()); //Show errors in new window and seperated by new line
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
        var vm = ServiceContainer.Current.Resolve<AddDeviceWindowViewModel>();
        vm.ParentViewModel = this;

        ApplyViewModel(wndw, vm);

        wndw.ShowDialog(((IClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime).MainWindow);
    }
}
