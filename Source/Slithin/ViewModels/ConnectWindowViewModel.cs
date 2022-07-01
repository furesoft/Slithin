using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using LiteDB;
using Material.Styles;
using Renci.SshNet;
using Serilog;
using Slithin.Core;
using Slithin.Core.MVVM;
using Slithin.Core.Services;
using Slithin.Core.Services.Implementations;
using Slithin.Core.Sync;
using Slithin.Models;
using Slithin.UI.Views;
using Slithin.Validators;

namespace Slithin.ViewModels;

public class ConnectionWindowViewModel : BaseViewModel
{
    private readonly ILocalisationService _localisationService;
    private readonly ILoginService _loginService;
    private readonly IScreenRememberService _screenRememberService;
    private readonly ISettingsService _settingsService;
    private readonly LoginInfoValidator _validator;
    private ObservableCollection<LoginInfo> _loginCredentials;

    private LoginInfo _selectedLogin;

    public ConnectionWindowViewModel(ILoginService loginService,
                                     LoginInfoValidator validator,
                                     ILocalisationService localisationService,
                                     ISettingsService settingsService,
                                     IScreenRememberService screenRememberService)
    {
        _loginService = loginService;
        _validator = validator;
        _localisationService = localisationService;
        _settingsService = settingsService;
        _screenRememberService = screenRememberService;
        ConnectCommand = new DelegateCommand(Connect);
        HelpCommand = new DelegateCommand(Help);
        OpenAddDeviceCommand = new DelegateCommand(OpenAddDevice);

        SelectedLogin = new LoginInfo();
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

        LoginCredentials = new(li);

        if (_screenRememberService.HasMultipleScreens() && _settingsService.GetSettings().UsedMultiScreen)
        {
            _screenRememberService.Remember();
        }
    }

    private void client_errocOccured(object s, Renci.SshNet.Common.ExceptionEventArgs _)
    {
        DialogService.OpenError(_.Exception.ToString());

        var logger = ServiceLocator.Container.Resolve<ILogger>();

        logger.Error(_.Exception.ToString());
    }

    private void Connect(object obj)
    {
        ServiceLocator.Container.Resolve<LogInitalizer>().Init();

        var logger = ServiceLocator.Container.Resolve<ILogger>();
        var discovery = ServiceLocator.Container.Resolve<IDeviceDiscovery>();
        var loginService = ServiceLocator.Container.Resolve<ILoginService>();

        SshClient client = null;
        ScpClient scp = null;

        var validationResult = _validator.Validate(SelectedLogin);

        if (!validationResult.IsValid)
        {
            DialogService.OpenError(string.Join("\n", validationResult.Errors));
            return;
        }

        if (!discovery.PingDevice(System.Net.IPAddress.Parse(SelectedLogin.IP))
            && SelectedLogin.Name != _localisationService.GetString("No Device Saved"))
        {
            var newIP = discovery.Discover();

            if (newIP != default)
            {
                SelectedLogin.IP = newIP.ToString();

                //Replace Old IP To New IP
                loginService.SetLoginCredential(SelectedLogin);
                loginService.UpdateIPAfterUpdate();
            }
            else
            {
                const string message = "Your remarkable is not reachable. Please check your connection and restart Slithin";

                NotificationService.Show(_localisationService.GetString(
                    message));
                logger.Warning(message);
            }
        }

        IPAddress ip = IPAddress.Parse(SelectedLogin.IP);

        if (SelectedLogin.UsesKey)
        {
            client = new SshClient(ip.Address, ip.Port, "root", SelectedLogin.GetKey());
            scp = new ScpClient(ip.Address, ip.Port, "root", SelectedLogin.GetKey());
        }
        else
        {
            if (string.IsNullOrEmpty(SelectedLogin.Name))
            {
                SelectedLogin.Name = "DefaultDevice";
            }

            client = new SshClient(ip.Address, ip.Port, "root", SelectedLogin.Password);
            scp = new ScpClient(ip.Address, ip.Port, "root", SelectedLogin.Password);
        }

        client.ErrorOccurred += client_errocOccured;

        try
        {
            client.Connect();
            scp.Connect();

            if (!client.IsConnected)
            {
                SnackbarHost.Post(_localisationService.GetString("Could not connect to host"));
                return;
            }

            if (Application.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            {
                return;
            }

            ServiceLocator.Container.Register(new SshServiceImpl(client, scp));

            ServiceLocator.SyncService = new SynchronisationService(ServiceLocator.Container.Resolve<LiteDatabase>());

            ServiceLocator.Container.Resolve<IMailboxService>().Init();
            ServiceLocator.Container.Resolve<IMailboxService>().InitMessageRouter();

            ServiceLocator.Container.Resolve<IContextMenuProvider>().Init();

            _loginService.SetLoginCredential(SelectedLogin);

            client.ErrorOccurred -= client_errocOccured;

            desktop.MainWindow.Hide();
            desktop.MainWindow = new MainWindow();

            desktop.MainWindow.Show();
        }
        catch (Exception ex)
        {
            SnackbarHost.Post(_localisationService.GetString("Could not connect to host"));
            logger.Error(ex.ToString());
        }
    }

    private void Help(object obj)
    {
        Utils.OpenUrl("https://tinyurl.com/remarkable-ssh");
    }

    private void OpenAddDevice(object obj)
    {
        var wndw = new AddDeviceWindow();
        var vm = ServiceLocator.Container.Resolve<AddDeviceWindowViewModel>();
        vm.ParentViewModel = this;

        wndw.DataContext = vm;

        vm.OnRequestClose += () => wndw.Close();

        wndw.ShowDialog(((IClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime).MainWindow);
    }
}
