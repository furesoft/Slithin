using System;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Input;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI.Fody.Helpers;
using Renci.SshNet;
using Renci.SshNet.Common;
using Slithin.Controls;
using Slithin.Core;
using Slithin.Core.Scripting;
using Slithin.Core.Services;
using Slithin.Core.Sync;
using Slithin.Core.Validators;
using Slithin.UI.Views;

namespace Slithin.ViewModels
{
    public class ConnectionWindowViewModel : BaseViewModel
    {
        private readonly EventStorage _events;
        private readonly ILoginService _loginService;
        private readonly ISettingsService _settingsService;
        private readonly LoginInfoValidator _validator;

        private string _ip;

        private string _password;

        private bool _remember;

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
        }

        public ICommand ConnectCommand { get; set; }

        public ICommand HelpCommand { get; set; }

        public string IP
        {
            get { return _ip; }
            set { SetValue(ref _ip, value); }
        }

        [Reactive]
        public string IP { get; set; }

        [Reactive]
        public string Password { get; set; }

        [Reactive]
        public bool Remember { get; set; }

        private void Connect(object obj)
        {
            ServiceLocator.Container.Register(new SshClient(IP, 22, "root", Password));
            ServiceLocator.Container.Register(new ScpClient(IP, 22, "root", Password));

            var client = ServiceLocator.Container.Resolve<SshClient>();
            //SyncService = new(pathManager, Container.Resolve<LiteDatabase>(), Container.Resolve<LocalRepository>(), Container.Resolve<ILoadingService>());
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

            var loginInfo = new LoginInfo(IP, Password, Remember);
            var validationResult = _validator.Validate(loginInfo);

            if (validationResult.IsValid)
            {
                try
                {
                    client.Connect();
                    ServiceLocator.Container.Resolve<ScpClient>().Connect();

                    if (client.IsConnected)
                    {
                        if (Remember)
                        {
                            _loginService.RememberLoginCredencials(loginInfo);
                        }

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
                                var galleryWindow = new GalleryWindow();
                                //galleryWindow.DataContext = some images
                                settings.HasFirstGalleryShown = true;
                                //_settingsService.Save(settings);

                                galleryWindow.Show();
                            }
                        }
                    }
                    else
                    {
                        DialogService.OpenError("Could not connect to host");
                    }
                }
                catch (SshException ex)
                {
                    DialogService.OpenError(ex.Message);
                }
            }
            else
            {
                DialogService.OpenError(string.Join("\n", validationResult.Errors));
            }
        }

        private void Help(object obj)
        {
            Utils.OpenUrl("http://www.remarkablewiki.com/");
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
