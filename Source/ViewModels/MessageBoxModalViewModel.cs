using System.Windows.Input;
using Slithin.Core;

namespace Slithin.ViewModels
{
    public class MessageBoxModalViewModel : BaseViewModel
    {
        private string _message;

        public ICommand AcceptCommand { get; set; }

        public string Message
        {
            get { return _message; }
            set { SetValue(ref _message, value); }
        }
    }
}
