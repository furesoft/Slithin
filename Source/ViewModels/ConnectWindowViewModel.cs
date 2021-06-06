using System;
using System.Windows.Input;

namespace Slithin.ViewModels
{
    public class ConnectionWindowViewModel : BaseViewModel
    {
        public ConnectionWindowViewModel()
        {
            _ipAddress = "";
            _userName = "";
            _password = "";

            ConnectCommand = new DelegateCommand(Connect);
        }

        private void Connect(object? obj)
        {
            Console.WriteLine("Connect clicked");
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

        public ICommand ConnectCommand { get; set; }

    }
}