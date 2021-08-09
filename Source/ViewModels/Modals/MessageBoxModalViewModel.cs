using System.Windows.Input;
using Slithin.Core;
using Slithin.ViewModels;

namespace Slithin.ViewModels.Modals
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
