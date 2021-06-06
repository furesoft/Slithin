using System;
using System.Windows.Input;

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