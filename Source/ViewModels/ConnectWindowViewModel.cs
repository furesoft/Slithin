using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Input;
using Avalonia.Controls.ApplicationLifetimes;
using Material.Styles;
using Renci.SshNet;
using Slithin.Controls;
using Slithin.Core;
using Slithin.Core.Scripting;
using Slithin.Core.Services;
using Slithin.Core.Sync;
using Slithin.Core.Validators;
using Slithin.UI.GalleryFirstStart;
using Slithin.UI.UpdateGallery;
using Slithin.UI.Views;
using Slithin.Models;
using System.Collections.ObjectModel;

namespace Slithin.ViewModels
{
    public class ConnectionWindowViewModel : BaseViewModel
    {
        private readonly EventStorage _events;
        private readonly ILoginService _loginService;
        private readonly ISettingsService _settingsService;
        private readonly LoginInfoValidator _validator;

        private ObservableCollection<LoginInfo> _loginCredentials;

        private LoginInfo _selectedLogin;

        public ConnectionWindowViewModel(EventStorage events,
                                         ILoginService loginService,
                                         LoginInfoValidator validator,
                                         ISettingsService settingsService)
        {
            _events = events;
            _loginService = loginService;
            _validator = validator;
            _settingsService = settingsService;

            ConnectCommand = new DelegateCommand(Connect);
            HelpCommand = new DelegateCommand(Help);

            SelectedLogin = new("", "");
        }

        public ICommand ConnectCommand { get; set; }

        public ICommand HelpCommand { get; set; }

        public ObservableCollection<LoginInfo> LoginCredentials
        {
            get { return _loginCredentials; }
            set { SetValue(ref _loginCredentials, value); }
        }

        public LoginInfo SelectedLogin
        {
            get { return _selectedLogin; }
            set { SetValue(ref _selectedLogin, value); }
        }

        private void Connect(object obj)
        {
            var validationResult = _validator.Validate(SelectedLogin);

            if (validationResult.IsValid)
            {
                ServiceLocator.Container.Register(new SshClient(SelectedLogin.IP, 22, "root", SelectedLogin.Password));
                ServiceLocator.Container.Register(new ScpClient(SelectedLogin.IP, 22, "root", SelectedLogin.Password));

                var client = ServiceLocator.Container.Resolve<SshClient>();

                ServiceLocator.SyncService = ServiceLocator.Container.Resolve<SynchronisationService>();
                ServiceLocator.Container.Register<Automation>().AsSingleton();

                var automation = ServiceLocator.Container.Resolve<Automation>();

                automation.Init();
                automation.Evaluate("testScript");

                ServiceLocator.Container.Resolve<IMailboxService>().Init();
                ServiceLocator.Container.Resolve<IMailboxService>().InitMessageRouter();

                client.ErrorOccurred += (s, _) =>
                {
                    DialogService.OpenError(_.Exception.ToString());
                };

                try
                {
                    client.Connect();
                    ServiceLocator.Container.Resolve<ScpClient>().Connect();

                    if (client.IsConnected)
                    {
                        if (App.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                        {
                            _events.Invoke("connect");

                            var pingTimer = new System.Timers.Timer();
                            pingTimer.Elapsed += pingTimer_ellapsed;
                            pingTimer.Interval = TimeSpan.FromMinutes(5).TotalMilliseconds;
                            pingTimer.Start();

                            desktop.MainWindow.Hide();
                            desktop.MainWindow = new MainWindow();

                            desktop.MainWindow.Show();

                            var settings = _settingsService.Get();
                            if (!settings.HasFirstGalleryShown)
                            {
                                var vm = new GalleryWindowViewModel();
                                vm.Slides.Add(new WelcomePage());

                                var galleryWindow = new GalleryWindow(vm);

                                settings.HasFirstGalleryShown = true;
                                _settingsService.Save(settings);

                                galleryWindow.Show();
                            }

                            if (Environment.GetCommandLineArgs().Contains("-updateInstalled"))
                            {
                                var vm = new GalleryWindowViewModel();
                                vm.Slides.Add(new UpdateInstalledPage());

                                var galleryWindow = new GalleryWindow(vm);

                                galleryWindow.Show();
                            }
                        }
                    }
                    else
                    {
                        SnackbarHost.Post("Could not connect to host");
                    }
                }
                catch (Exception ex)
                {
                    SnackbarHost.Post(ex.Message);
                }
            }
            else
            {
                SnackbarHost.Post(string.Join("\n", validationResult.Errors));
            }
        }

        private void Help(object obj)
        {
            Utils.OpenUrl("https://tinyurl.com/remarkable-ssh");
        }

        private void pingTimer_ellapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var pingSender = new Ping();

            var data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            var buffer = Encoding.ASCII.GetBytes(data);

            var timeout = 10000;

            var options = new PingOptions(64, true);

            var reply = pingSender.Send(ServiceLocator.Container.Resolve<ScpClient>().ConnectionInfo.Host, timeout, buffer, options);

            if (reply.Status != IPStatus.Success)
            {
                NotificationService.Show("Your remarkable is not reachable. Please check your connection and restart Slithin");
            }
        }
    }
}
