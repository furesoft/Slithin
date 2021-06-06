using System;
using System.Windows.Input;
using Avalonia.Controls.ApplicationLifetimes;
using Renci.SshNet.Common;
using Slithin.Core;
using Slithin.Views;

namespace Slithin.ViewModels
{
    public class ConnectionWindowViewModel : BaseViewModel
    {
        public ConnectionWindowViewModel()
        {
            _ipAddress = string.Empty;
            _userName = string.Empty;
            _password = string.Empty;
            _remember = false;

            ConnectCommand = new DelegateCommand(Connect);
        }

        private void Connect(object? obj)
        {
            Console.WriteLine("Connect clicked");
            ServiceLocator.Client = new Renci.SshNet.SshClient(IP, 22, UserName, Password);
            ServiceLocator.Scp = new Renci.SshNet.ScpClient(IP, 22, UserName, Password);

            try
            {
                ServiceLocator.Client.Connect();

                if (ServiceLocator.Client.IsConnected)
                {
                    if (App.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                    {
                        desktop.MainWindow.Hide();
                        desktop.MainWindow = new MainWindow();
                        desktop.MainWindow.Show();
                    }
                }
                else
                {
                    //ToDo Display Connection Error
                    System.Console.WriteLine("Could not connect to host");
                }
            }
            catch (SshException ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }

        private string _ipAddress;
        public string IP
        {
            get { return _ipAddress; }
            set { SetValue(ref _ipAddress, value); }
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { SetValue(ref _userName, value); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { SetValue(ref _password, value); }
        }

        private bool _remember;
        public bool Remember
        {
            get { return _remember; }
            set { SetValue(ref _remember, value); }
        }


        public ICommand ConnectCommand { get; set; }

    }
}