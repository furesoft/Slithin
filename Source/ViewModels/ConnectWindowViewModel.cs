using System;
using System.Windows.Input;
using Avalonia.Controls.ApplicationLifetimes;
using LiteDB;
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
            _password = string.Empty;
            _remember = false;

            ConnectCommand = new DelegateCommand(Connect);
        }

        private void Connect(object? obj)
        {
            Console.WriteLine("Connect clicked");
            ServiceLocator.Client = new Renci.SshNet.SshClient(IP, 22, "root", Password);
            ServiceLocator.Scp = new Renci.SshNet.ScpClient(IP, 22, "root", Password);

            try
            {
                ServiceLocator.Client.Connect();

                if (ServiceLocator.Client.IsConnected)
                {
                    if (Remember)
                    {
                        ServiceLocator.RememberLoginCredencials(this);
                    }

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
                //ToDo Display Connection Error
                System.Console.WriteLine(ex.Message);
            }
        }

        private string _ipAddress;
        public string IP
        {
            get { return _ipAddress; }
            set { SetValue(ref _ipAddress, value); }
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


        [BsonIgnore]
        public ICommand ConnectCommand { get; set; }
        public BsonAutoId _id { get; set; }

    }
}