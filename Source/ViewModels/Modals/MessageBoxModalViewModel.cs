using System.Windows.Input;
using ReactiveUI.Fody.Helpers;
using Slithin.Core;

namespace Slithin.ViewModels.Modals
{
    public class MessageBoxModalViewModel : BaseViewModel
    {
        private string _message;
        public ICommand AcceptCommand { get; set; }

        [Reactive]
        public string Message { get; set; }
    }
}
