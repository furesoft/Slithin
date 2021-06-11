using System;
using System.Net;
using System.Windows.Input;
using Avalonia.Controls.ApplicationLifetimes;
using LiteDB;
using Renci.SshNet.Common;
using Slithin.Controls;
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
            ServiceLocator.Client = new Renci.SshNet.SshClient(IP, 22, "root", Password);
            ServiceLocator.Scp = new Renci.SshNet.ScpClient(IP, 22, "root", Password);

            if (IPAddress.TryParse(IP, out var addr))
            {
                try
                {
                    ServiceLocator.Client.Connect();
                    ServiceLocator.Scp.Connect();

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
                //ToDo Display invalid ip Error
                System.Console.WriteLine("The given IP was not valid");
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
        public ObjectId _id { get; set; }

    }
}