using System;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Text;
using System.Timers;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using LiteDB;
using Material.Styles;
using Renci.SshNet;
using Serilog;
using Slithin.Controls;
using Slithin.Core;
using Slithin.Core.Services;
using Slithin.Core.Sync;
using Slithin.Core.Validators;
using Slithin.Models;
using Slithin.UI.Views;

namespace Slithin.ViewModels;

public class ConnectionWindowViewModel : BaseViewModel
{
    private readonly ILoginService _loginService;
    private readonly ISettingsService _settingsService;
    private readonly LoginInfoValidator _validator;

    private ObservableCollection<LoginInfo> _loginCredentials;

    private LoginInfo _selectedLogin;

    public ConnectionWindowViewModel(ILoginService loginService,
        LoginInfoValidator validator,
        ISettingsService settingsService)
    {
        _loginService = loginService;
        _validator = validator;
        _settingsService = settingsService;

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

    private void Connect(object obj)
    {
        ServiceLocator.Container.Resolve<LogInitalizer>().Init();

        var logger = ServiceLocator.Container.Resolve<ILogger>();

        var validationResult = _validator.Validate(SelectedLogin);

        if (!validationResult.IsValid)
        {
            SnackbarHost.Post(string.Join("\n", validationResult.Errors));
            return;
        }

        var ip = IPAddress.Parse(SelectedLogin.IP);

        var client = new SshClient(ip.Address, ip.Port, "root", SelectedLogin.Password);
        var scp = new ScpClient(ip.Address, ip.Port, "root", SelectedLogin.Password);

        client.ErrorOccurred += (s, _) =>
        {
            DialogService.OpenError(_.Exception.ToString());
            logger.Error(_.Exception.ToString());
        };

        try
        {
            client.Connect();
            scp.Connect();

            if (!client.IsConnected)
            {
                SnackbarHost.Post("Could not connect to host");
                return;
            }

            if (Application.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            {
                return;
            }

            ServiceLocator.Container.Register(client);
            ServiceLocator.Container.Register(scp);

            ServiceLocator.SyncService = new SynchronisationService(ServiceLocator.Container.Resolve<LiteDatabase>());

            ServiceLocator.Container.Resolve<IMailboxService>().Init();
            ServiceLocator.Container.Resolve<IMailboxService>().InitMessageRouter();

            var pingTimer = new Timer();
            pingTimer.Elapsed += pingTimer_ellapsed;
            pingTimer.Interval = TimeSpan.FromMinutes(5).TotalMilliseconds;
            pingTimer.Start();

            _loginService.SetLoginCredential(SelectedLogin);

            desktop.MainWindow.Hide();
            desktop.MainWindow = new MainWindow();

            desktop.MainWindow.Show();
        }
        catch (Exception ex)
        {
            SnackbarHost.Post(ex.Message);
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

        wndw.Show();
    }

    private void pingTimer_ellapsed(object sender, ElapsedEventArgs e)
    {
        var pingSender = new Ping();

        var data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        var buffer = Encoding.ASCII.GetBytes(data);

        var timeout = 10000;

        var options = new PingOptions(64, true);

        var reply = pingSender.Send(ServiceLocator.Container.Resolve<ScpClient>().ConnectionInfo.Host, timeout, buffer,
            options);

        if (reply.Status != IPStatus.Success)
        {
            NotificationService.Show(
                "Your remarkable is not reachable. Please check your connection and restart Slithin");

            var logger = ServiceLocator.Container.Resolve<ILogger>();

            logger.Warning("Your remarkable is not reachable. Please check your connection and restart Slithin");
        }
    }
}
